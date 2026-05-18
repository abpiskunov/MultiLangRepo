#include "matrix.h"

namespace matheng {

Matrix::Matrix(int rows, int cols)
    : rows_(rows), cols_(cols), data_(rows, std::vector<double>(cols, 0.0)) {}

Matrix::Matrix(int rows, int cols, double initial_value)
    : rows_(rows), cols_(cols), data_(rows, std::vector<double>(cols, initial_value)) {}

double& Matrix::at(int row, int col) {
    if (row < 0 || row >= rows_ || col < 0 || col >= cols_) {
        throw std::out_of_range("Matrix index out of range");
    }
    return data_[row][col];
}

double Matrix::at(int row, int col) const {
    if (row < 0 || row >= rows_ || col < 0 || col >= cols_) {
        throw std::out_of_range("Matrix index out of range");
    }
    return data_[row][col];
}

Matrix Matrix::operator+(const Matrix& other) const {
    if (rows_ != other.rows_ || cols_ != other.cols_) {
        throw std::invalid_argument("Matrix dimensions must match for addition");
    }
    Matrix result(rows_, cols_);
    for (int i = 0; i < rows_; ++i) {
        for (int j = 0; j < cols_; ++j) {
            result.data_[i][j] = data_[i][j] + other.data_[i][j];
        }
    }
    return result;
}

Matrix Matrix::operator*(const Matrix& other) const {
    if (cols_ != other.rows_) {
        throw std::invalid_argument("Matrix dimensions incompatible for multiplication");
    }
    Matrix result(rows_, other.cols_);
    for (int i = 0; i < rows_; ++i) {
        for (int j = 0; j < other.cols_; ++j) {
            double sum = 0.0;
            for (int k = 0; k < cols_; ++k) {
                sum += data_[i][k] * other.data_[k][j];
            }
            result.data_[i][j] = sum;
        }
    }
    return result;
}

Matrix Matrix::transpose() const {
    Matrix result(cols_, rows_);
    for (int i = 0; i < rows_; ++i) {
        for (int j = 0; j < cols_; ++j) {
            result.data_[j][i] = data_[i][j];
        }
    }
    return result;
}

double Matrix::determinant() const {
    if (rows_ != cols_) {
        throw std::invalid_argument("Determinant is only defined for square matrices");
    }
    if (rows_ == 1) return data_[0][0];
    if (rows_ == 2) return data_[0][0] * data_[1][1] - data_[0][1] * data_[1][0];

    double det = 0.0;
    for (int j = 0; j < cols_; ++j) {
        Matrix sub(rows_ - 1, cols_ - 1);
        for (int si = 1; si < rows_; ++si) {
            int sj = 0;
            for (int k = 0; k < cols_; ++k) {
                if (k == j) continue;
                sub.data_[si - 1][sj++] = data_[si][k];
            }
        }
        det += (j % 2 == 0 ? 1 : -1) * data_[0][j] * sub.determinant();
    }
    return det;
}

Matrix Matrix::identity(int size) {
    Matrix m(size, size);
    for (int i = 0; i < size; ++i) {
        m.data_[i][i] = 1.0;
    }
    return m;
}

std::ostream& operator<<(std::ostream& os, const Matrix& m) {
    for (int i = 0; i < m.rows_; ++i) {
        os << "[ ";
        for (int j = 0; j < m.cols_; ++j) {
            os << m.data_[i][j];
            if (j < m.cols_ - 1) os << ", ";
        }
        os << " ]" << std::endl;
    }
    return os;
}

} // namespace matheng
