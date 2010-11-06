CSC = "gmcs"
BIN_DIR = "bin"
SRC_DIR = "src"
RUNTIME = "mono"

MAIN_PROJ = "Rhea"
MAIN_BIN = "#{BIN_DIR}/#{MAIN_PROJ}.exe"
MAIN_SRC_FILE_PAT = "#{SRC_DIR}/#{MAIN_PROJ}/**/*.cs"
MAIN_SRC_FILES = FileList[MAIN_SRC_FILE_PAT]
MAIN_OPTIONS = "-target:exe -out:#{MAIN_BIN}"

task "default" => "build"

task "build" => [BIN_DIR, MAIN_BIN]

directory BIN_DIR

file MAIN_BIN => MAIN_SRC_FILES do
  sh "#{CSC} #{MAIN_OPTIONS} #{MAIN_SRC_FILES}"
end

task "run" => "build" do
  sh "#{RUNTIME} #{MAIN_BIN}"
end

task "countline" do
  total = MAIN_SRC_FILES.inject(0) do |sum, name|
    print "#{name}: "
    puts count = IO.readlines(name).size
    sum + count
  end
  puts "Total: #{total}"
end
