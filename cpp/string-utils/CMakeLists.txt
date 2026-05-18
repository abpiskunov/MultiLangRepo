cmake_minimum_required(VERSION 3.14)
project(string-utils VERSION 1.0.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_library(string-utils
    src/string_utils.cpp
    src/tokenizer.cpp
)

target_include_directories(string-utils PUBLIC include)

add_executable(string-utils-demo src/main.cpp)
target_link_libraries(string-utils-demo PRIVATE string-utils)
