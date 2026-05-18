#include "expression_parser.h"
#include <sstream>
#include <stdexcept>
#include <cctype>
#include <cmath>

namespace matheng {

ExpressionParser::ExpressionParser() {}

void ExpressionParser::set_variable(const std::string& name, double value) {
    variables_[name] = value;
}

int ExpressionParser::precedence(char op) const {
    if (op == '+' || op == '-') return 1;
    if (op == '*' || op == '/') return 2;
    if (op == '^') return 3;
    return 0;
}

double ExpressionParser::apply_op(double a, double b, char op) const {
    switch (op) {
        case '+': return a + b;
        case '-': return a - b;
        case '*': return a * b;
        case '/':
            if (b == 0) throw std::runtime_error("Division by zero");
            return a / b;
        case '^': return std::pow(a, b);
        default: throw std::runtime_error(std::string("Unknown operator: ") + op);
    }
}

double ExpressionParser::parse_number(const std::string& expr, size_t& pos) const {
    size_t start = pos;
    while (pos < expr.size() && (std::isdigit(expr[pos]) || expr[pos] == '.')) {
        ++pos;
    }
    return std::stod(expr.substr(start, pos - start));
}

double ExpressionParser::evaluate(const std::string& expression) const {
    std::stack<double> values;
    std::stack<char> ops;

    for (size_t i = 0; i < expression.size(); ++i) {
        char c = expression[i];

        if (std::isspace(c)) continue;

        if (std::isdigit(c) || c == '.') {
            values.push(parse_number(expression, i));
            --i; // loop increment will advance
        }
        else if (std::isalpha(c)) {
            std::string var_name;
            while (i < expression.size() && std::isalpha(expression[i])) {
                var_name += expression[i++];
            }
            --i;
            auto it = variables_.find(var_name);
            if (it == variables_.end()) {
                throw std::runtime_error("Undefined variable: " + var_name);
            }
            values.push(it->second);
        }
        else if (c == '(') {
            ops.push(c);
        }
        else if (c == ')') {
            while (!ops.empty() && ops.top() != '(') {
                double b = values.top(); values.pop();
                double a = values.top(); values.pop();
                values.push(apply_op(a, b, ops.top()));
                ops.pop();
            }
            if (!ops.empty()) ops.pop(); // remove '('
        }
        else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^') {
            while (!ops.empty() && ops.top() != '(' && precedence(ops.top()) >= precedence(c)) {
                double b = values.top(); values.pop();
                double a = values.top(); values.pop();
                values.push(apply_op(a, b, ops.top()));
                ops.pop();
            }
            ops.push(c);
        }
    }

    while (!ops.empty()) {
        double b = values.top(); values.pop();
        double a = values.top(); values.pop();
        values.push(apply_op(a, b, ops.top()));
        ops.pop();
    }

    return values.top();
}

} // namespace matheng
