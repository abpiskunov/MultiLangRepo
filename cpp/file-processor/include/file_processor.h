#pragma once
#include <string>
#include <vector>
#include <fstream>

namespace fileproc {

class FileReader {
public:
    static std::string read_all(const std::string& path);
    static std::vector<std::string> read_lines(const std::string& path);
    static std::vector<std::vector<std::string>> read_csv(const std::string& path, char delimiter = ',');
    static size_t count_lines(const std::string& path);
    static bool file_exists(const std::string& path);
};

class CsvWriter {
public:
    explicit CsvWriter(const std::string& path, char delimiter = ',');
    ~CsvWriter();

    void write_header(const std::vector<std::string>& headers);
    void write_row(const std::vector<std::string>& values);
    void flush();

private:
    std::ofstream file_;
    char delimiter_;
    std::string escape(const std::string& value) const;
};

class PathUtils {
public:
    static std::string get_extension(const std::string& path);
    static std::string get_filename(const std::string& path);
    static std::string get_directory(const std::string& path);
    static std::string join(const std::string& base, const std::string& relative);
    static std::string normalize(const std::string& path);
};

} // namespace fileproc
