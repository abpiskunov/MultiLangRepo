#include <iostream>
#include <vector>
#include "matrix.h"
#include "statistics.h"
#include "expression_parser.h"

int main() {
    using namespace matheng;

    std::cout << "=== Matrix Demo ===" << std::endl;
    auto identity = Matrix::identity(3);
    std::cout << "3x3 Identity Matrix:" << std::endl << identity;

    Matrix a(2, 2);
    a.at(0, 0) = 1; a.at(0, 1) = 2;
    a.at(1, 0) = 3; a.at(1, 1) = 4;
    std::cout << "\nMatrix A:" << std::endl << a;
    std::cout << "Determinant: " << a.determinant() << std::endl;
    std::cout << "Transpose:" << std::endl << a.transpose();

    std::cout << "\n=== Statistics Demo ===" << std::endl;
    std::vector<double> data = {4.0, 8.0, 15.0, 16.0, 23.0, 42.0};
    std::cout << "Data: {4, 8, 15, 16, 23, 42}" << std::endl;
    std::cout << "Mean:     " << Statistics::mean(data) << std::endl;
    std::cout << "Median:   " << Statistics::median(data) << std::endl;
    std::cout << "StdDev:   " << Statistics::standard_deviation(data) << std::endl;
    std::cout << "P90:      " << Statistics::percentile(data, 90) << std::endl;

    std::vector<double> x = {1, 2, 3, 4, 5};
    std::vector<double> y = {2, 4, 5, 4, 5};
    auto [slope, intercept] = Statistics::linear_regression(x, y);
    std::cout << "\nLinear regression: y = " << slope << "x + " << intercept << std::endl;

    std::cout << "\n=== Expression Parser Demo ===" << std::endl;
    ExpressionParser parser;
    parser.set_variable("x", 10);
    parser.set_variable("y", 5);

    std::cout << "2 + 3 * 4 = " << parser.evaluate("2 + 3 * 4") << std::endl;
    std::cout << "(2 + 3) * 4 = " << parser.evaluate("(2 + 3) * 4") << std::endl;
    std::cout << "x + y * 2 = " << parser.evaluate("x + y * 2") << std::endl;
    std::cout << "2 ^ 10 = " << parser.evaluate("2 ^ 10") << std::endl;

    return 0;
}
