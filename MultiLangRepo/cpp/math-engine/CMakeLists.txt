cmake_minimum_required(VERSION 3.14)
project(math-engine VERSION 1.0.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_library(math-engine
    src/matrix.cpp
    src/statistics.cpp
    src/expression_parser.cpp
)

target_include_directories(math-engine PUBLIC include)

add_executable(math-engine-demo src/main.cpp)
target_link_libraries(math-engine-demo PRIVATE math-engine)
