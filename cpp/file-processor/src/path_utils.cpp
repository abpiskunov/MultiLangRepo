#include "file_processor.h"
#include <algorithm>

namespace fileproc {

std::string PathUtils::get_extension(const std::string& path) {
    auto dot_pos = path.rfind('.');
    if (dot_pos == std::string::npos || dot_pos == 0) return "";
    auto slash_pos = path.find_last_of("/\\");
    if (slash_pos != std::string::npos && dot_pos < slash_pos) return "";
    return path.substr(dot_pos);
}

std::string PathUtils::get_filename(const std::string& path) {
    auto pos = path.find_last_of("/\\");
    if (pos == std::string::npos) return path;
    return path.substr(pos + 1);
}

std::string PathUtils::get_directory(const std::string& path) {
    auto pos = path.find_last_of("/\\");
    if (pos == std::string::npos) return ".";
    return path.substr(0, pos);
}

std::string PathUtils::join(const std::string& base, const std::string& relative) {
    if (base.empty()) return relative;
    char last = base.back();
    if (last == '/' || last == '\\') {
        return base + relative;
    }
    return base + "/" + relative;
}

std::string PathUtils::normalize(const std::string& path) {
    std::string result = path;
    std::replace(result.begin(), result.end(), '\\', '/');
    // Remove trailing slash
    while (result.size() > 1 && result.back() == '/') {
        result.pop_back();
    }
    return result;
}

} // namespace fileproc
