#include "file_processor.h"
#include <stdexcept>

namespace fileproc {

CsvWriter::CsvWriter(const std::string& path, char delimiter)
    : file_(path), delimiter_(delimiter) {
    if (!file_.is_open()) {
        throw std::runtime_error("Cannot open file for writing: " + path);
    }
}

CsvWriter::~CsvWriter() {
    if (file_.is_open()) {
        file_.close();
    }
}

void CsvWriter::write_header(const std::vector<std::string>& headers) {
    write_row(headers);
}

void CsvWriter::write_row(const std::vector<std::string>& values) {
    for (size_t i = 0; i < values.size(); ++i) {
        if (i > 0) file_ << delimiter_;
        file_ << escape(values[i]);
    }
    file_ << "\n";
}

void CsvWriter::flush() {
    file_.flush();
}

std::string CsvWriter::escape(const std::string& value) const {
    if (value.find(delimiter_) != std::string::npos ||
        value.find('"') != std::string::npos ||
        value.find('\n') != std::string::npos) {
        std::string escaped = "\"";
        for (char c : value) {
            if (c == '"') escaped += "\"\"";
            else escaped += c;
        }
        escaped += "\"";
        return escaped;
    }
    return value;
}

} // namespace fileproc
