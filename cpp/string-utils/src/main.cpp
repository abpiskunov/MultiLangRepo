#include <iostream>
#include "string_utils.h"
#include "tokenizer.h"

int main() {
    using namespace strutils;

    std::cout << "=== String Utils Demo ===" << std::endl;

    std::string input = "  Hello, World!  ";
    std::cout << "Original: '" << input << "'" << std::endl;
    std::cout << "Trimmed:  '" << StringUtils::trim(input) << "'" << std::endl;
    std::cout << "Upper:    '" << StringUtils::to_upper(input) << "'" << std::endl;
    std::cout << "Lower:    '" << StringUtils::to_lower(input) << "'" << std::endl;

    std::string csv = "apple,banana,cherry,date";
    auto parts = StringUtils::split(csv, ',');
    std::cout << "\nSplit '" << csv << "':" << std::endl;
    for (const auto& part : parts) {
        std::cout << "  - " << part << std::endl;
    }

    std::cout << "Joined: " << StringUtils::join(parts, " | ") << std::endl;

    std::string palindrome = "A man, a plan, a canal: Panama";
    std::cout << "\n'" << palindrome << "' is palindrome: "
              << (StringUtils::is_palindrome(palindrome) ? "true" : "false") << std::endl;

    std::cout << "\n=== Tokenizer Demo ===" << std::endl;
    Tokenizer tokenizer;
    std::string text = "The quick brown fox jumps over the lazy dog";
    auto tokens = tokenizer.tokenize_with_positions(text);
    std::cout << "Tokens in '" << text << "':" << std::endl;
    for (const auto& [token, pos] : tokens) {
        std::cout << "  [" << pos << "] " << token << std::endl;
    }

    return 0;
}
