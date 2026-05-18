cmake_minimum_required(VERSION 3.14)
project(file-processor VERSION 1.0.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_library(file-processor
    src/file_reader.cpp
    src/csv_writer.cpp
    src/path_utils.cpp
)

target_include_directories(file-processor PUBLIC include)

add_executable(file-processor-demo src/main.cpp)
target_link_libraries(file-processor-demo PRIVATE file-processor)
