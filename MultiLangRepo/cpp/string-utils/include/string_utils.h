#pragma once
#include <string>
#include <vector>
#include <algorithm>

namespace strutils {

class StringUtils {
public:
    static std::string trim(const std::string& str);
    static std::string to_upper(const std::string& str);
    static std::string to_lower(const std::string& str);
    static std::vector<std::string> split(const std::string& str, char delimiter);
    static std::string join(const std::vector<std::string>& parts, const std::string& separator);
    static bool starts_with(const std::string& str, const std::string& prefix);
    static bool ends_with(const std::string& str, const std::string& suffix);
    static std::string replace_all(const std::string& str, const std::string& from, const std::string& to);
    static std::string repeat(const std::string& str, int count);
    static bool is_palindrome(const std::string& str);
};

} // namespace strutils
