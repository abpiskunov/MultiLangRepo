#include "file_processor.h"
#include <sstream>
#include <stdexcept>

namespace fileproc {

std::string FileReader::read_all(const std::string& path) {
    std::ifstream file(path);
    if (!file.is_open()) {
        throw std::runtime_error("Cannot open file: " + path);
    }
    std::ostringstream ss;
    ss << file.rdbuf();
    return ss.str();
}

std::vector<std::string> FileReader::read_lines(const std::string& path) {
    std::ifstream file(path);
    if (!file.is_open()) {
        throw std::runtime_error("Cannot open file: " + path);
    }
    std::vector<std::string> lines;
    std::string line;
    while (std::getline(file, line)) {
        lines.push_back(line);
    }
    return lines;
}

std::vector<std::vector<std::string>> FileReader::read_csv(const std::string& path, char delimiter) {
    auto lines = read_lines(path);
    std::vector<std::vector<std::string>> rows;
    for (const auto& line : lines) {
        std::vector<std::string> row;
        std::istringstream stream(line);
        std::string cell;
        while (std::getline(stream, cell, delimiter)) {
            row.push_back(cell);
        }
        rows.push_back(row);
    }
    return rows;
}

size_t FileReader::count_lines(const std::string& path) {
    return read_lines(path).size();
}

bool FileReader::file_exists(const std::string& path) {
    std::ifstream file(path);
    return file.good();
}

} // namespace fileproc
