#pragma once
#include <string>
#include <vector>

namespace strutils {

class Tokenizer {
public:
    explicit Tokenizer(const std::string& delimiters = " \t\n\r");

    std::vector<std::string> tokenize(const std::string& input) const;
    std::vector<std::pair<std::string, int>> tokenize_with_positions(const std::string& input) const;
    int count_tokens(const std::string& input) const;

private:
    std::string delimiters_;
    bool is_delimiter(char c) const;
};

} // namespace strutils
