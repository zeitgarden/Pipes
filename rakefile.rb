require 'rubygems'
require 'albacore'
require 'rake/clean'
include FileTest
load "version.txt"

COMPILE_TARGET = (ENV['config'] || "Debug")
CLR_TOOLS_VERSION = "v4.0.30319"
WORKING_DIR = File.dirname(__FILE__)

buildsupportfiles = Dir["#{WORKING_DIR}/buildsupport/*.rb"]
raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?
buildsupportfiles.each { |ext| load ext }

tools = Dir["#{WORKING_DIR}/tools/*.rb"]
tools.each { |ext| load ext }


ARTIFACTS = "artifacts"
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
COPYRIGHT = 'Copyright 2012, ALS. All rights reserved.';
PRODUCT = "Pipes"
RESULTS_DIR = "results"
SRC_DIR = "src"
STAGE_DIR = "build"

tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')
BUILD_NUMBER = "#{BUILD_VERSION}.#{build_revision}"


# Add directories to Rake's clean task
CLEAN.include(STAGE_DIR, ARTIFACTS)

desc "Displays a list of tasks"
task :help do
  taskHash = Hash[*(`rake.bat -T`.split(/\n/).collect { |l| l.match(/rake (\S+)\s+\#\s(.+)/).to_a }.collect { |l| [l[1], l[2]] }).flatten]

  indent = " " * 26

  puts "rake #{indent}#Runs the 'default' task"

  taskHash.each_pair do |key, value|
    if key.nil?
      next
    end
    puts "rake #{key}#{indent.slice(0, indent.length - key.length)}##{value}"
  end
end

desc "Compiles, unit tests, generates the database"
task :all => [:default]

desc "**Default**, compiles and runs tests"
task :default => [:compile, :unit_test]

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_VERSION + ".0"

  begin
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
  end
  puts "##teamcity[buildNumber '#{BUILD_NUMBER}']" unless tc_build_number.nil?
  puts "Version: #{BUILD_NUMBER}" if tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = PRODUCT
  asm.description = BUILD_NUMBER
  asm.version = asm_version
  asm.file_version = BUILD_NUMBER
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO

end

def gittag
  return @gittag if @gittag

  # needed until teamcity provides a way to pull tags
  `git fetch` if ENV["BUILD_NUMBER"]

  description = `git describe --long`.chomp # looks something like v0.1.0-63-g92228f4

  versionpart = /^v?(\d+)\.(\d+)\.(\d+)-(\d+)-/.match(description)
  @gittag = versionpart.nil? ? [0, 0, 0, 0] : versionpart[1..5]
end

desc "Compiles the app"
task :compile => [:clean, :restore_if_missing, :version] do
  MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => 'src/Pipes.sln', :clrversion => CLR_TOOLS_VERSION
  AspNetCompilerRunner.compile :webPhysDir => "src/HelloWorld.WebApp", :webVirDir => "localhost/xyzzyplugh"

  mkdir_p STAGE_DIR
  copyOutputFiles "src/Pipes/bin/#{COMPILE_TARGET}", "Pipes.{dll,pdb}", STAGE_DIR

end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)){|file|
        copy(file, outDir) if File.file?(file)
  }
end

desc "Runs unit tests"
task :test => [:unit_test]

desc "Runs unit tests"
task :unit_test => :compile do
   runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
   runner.executeTests ["Pipes.Tests"]
end

desc "Target used for the CI server"
task :ci => [:default,:create_package]

desc "ZIPs up the build results"
zip :create_package do |zip|
        mkdir_p ARTIFACTS
        zip.directories_to_zip = [STAGE_DIR]
        zip.output_file = 'Pipes.'+ BUILD_NUMBER + '.zip'
        zip.output_path = [ARTIFACTS]
end