#include "tokenizer.h"

namespace strutils {

Tokenizer::Tokenizer(const std::string& delimiters) : delimiters_(delimiters) {}

bool Tokenizer::is_delimiter(char c) const {
    return delimiters_.find(c) != std::string::npos;
}

std::vector<std::string> Tokenizer::tokenize(const std::string& input) const {
    std::vector<std::string> tokens;
    std::string current;

    for (char c : input) {
        if (is_delimiter(c)) {
            if (!current.empty()) {
                tokens.push_back(current);
                current.clear();
            }
        } else {
            current += c;
        }
    }

    if (!current.empty()) {
        tokens.push_back(current);
    }

    return tokens;
}

std::vector<std::pair<std::string, int>> Tokenizer::tokenize_with_positions(const std::string& input) const {
    std::vector<std::pair<std::string, int>> tokens;
    std::string current;
    int start_pos = -1;

    for (int i = 0; i < static_cast<int>(input.size()); ++i) {
        if (is_delimiter(input[i])) {
            if (!current.empty()) {
                tokens.emplace_back(current, start_pos);
                current.clear();
                start_pos = -1;
            }
        } else {
            if (current.empty()) {
                start_pos = i;
            }
            current += input[i];
        }
    }

    if (!current.empty()) {
        tokens.emplace_back(current, start_pos);
    }

    return tokens;
}

int Tokenizer::count_tokens(const std::string& input) const {
    return static_cast<int>(tokenize(input).size());
}

} // namespace strutils
