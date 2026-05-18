#pragma once
#include <string>
#include <stack>
#include <map>

namespace matheng {

class ExpressionParser {
public:
    ExpressionParser();
    double evaluate(const std::string& expression) const;
    void set_variable(const std::string& name, double value);

private:
    std::map<std::string, double> variables_;
    int precedence(char op) const;
    double apply_op(double a, double b, char op) const;
    double parse_number(const std::string& expr, size_t& pos) const;
};

} // namespace matheng
