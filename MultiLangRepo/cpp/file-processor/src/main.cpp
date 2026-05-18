#include <iostream>
#include "file_processor.h"

int main() {
    using namespace fileproc;

    std::cout << "=== Path Utils Demo ===" << std::endl;

    std::string path = "/home/user/documents/report.csv";
    std::cout << "Path:      " << path << std::endl;
    std::cout << "Filename:  " << PathUtils::get_filename(path) << std::endl;
    std::cout << "Directory: " << PathUtils::get_directory(path) << std::endl;
    std::cout << "Extension: " << PathUtils::get_extension(path) << std::endl;

    std::string joined = PathUtils::join("/home/user", "documents/report.csv");
    std::cout << "Joined:    " << joined << std::endl;

    std::string messy = "C:\\Users\\user\\Documents\\..\\report.csv";
    std::cout << "Normalized: " << PathUtils::normalize(messy) << std::endl;

    std::cout << "\n=== CSV Writer Demo ===" << std::endl;
    {
        CsvWriter writer("demo_output.csv");
        writer.write_header({"Name", "Age", "City"});
        writer.write_row({"Alice", "30", "New York"});
        writer.write_row({"Bob", "25", "San Francisco"});
        writer.write_row({"Charlie", "35", "Chicago"});
        writer.flush();
        std::cout << "Wrote demo_output.csv" << std::endl;
    }

    std::cout << "\n=== File Reader Demo ===" << std::endl;
    if (FileReader::file_exists("demo_output.csv")) {
        auto lines = FileReader::read_lines("demo_output.csv");
        std::cout << "Lines in demo_output.csv: " << lines.size() << std::endl;
        for (const auto& line : lines) {
            std::cout << "  " << line << std::endl;
        }

        auto csv = FileReader::read_csv("demo_output.csv");
        std::cout << "\nParsed CSV rows: " << csv.size() << std::endl;
    }

    return 0;
}
