#pragma once
#include <vector>
#include <stdexcept>
#include <iostream>

namespace matheng {

class Matrix {
public:
    Matrix(int rows, int cols);
    Matrix(int rows, int cols, double initial_value);

    int rows() const { return rows_; }
    int cols() const { return cols_; }

    double& at(int row, int col);
    double at(int row, int col) const;

    Matrix operator+(const Matrix& other) const;
    Matrix operator*(const Matrix& other) const;
    Matrix transpose() const;
    double determinant() const;

    static Matrix identity(int size);

    friend std::ostream& operator<<(std::ostream& os, const Matrix& m);

private:
    int rows_, cols_;
    std::vector<std::vector<double>> data_;
};

} // namespace matheng
