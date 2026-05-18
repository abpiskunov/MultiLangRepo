#include "statistics.h"
#include <algorithm>
#include <numeric>
#include <stdexcept>

namespace matheng {

double Statistics::mean(const std::vector<double>& data) {
    if (data.empty()) throw std::invalid_argument("Cannot compute mean of empty dataset");
    return std::accumulate(data.begin(), data.end(), 0.0) / data.size();
}

double Statistics::median(std::vector<double> data) {
    if (data.empty()) throw std::invalid_argument("Cannot compute median of empty dataset");
    std::sort(data.begin(), data.end());
    size_t n = data.size();
    if (n % 2 == 0) {
        return (data[n / 2 - 1] + data[n / 2]) / 2.0;
    }
    return data[n / 2];
}

double Statistics::variance(const std::vector<double>& data) {
    if (data.size() < 2) throw std::invalid_argument("Need at least 2 data points for variance");
    double m = mean(data);
    double sum = 0.0;
    for (double x : data) {
        sum += (x - m) * (x - m);
    }
    return sum / (data.size() - 1);
}

double Statistics::standard_deviation(const std::vector<double>& data) {
    return std::sqrt(variance(data));
}

double Statistics::min(const std::vector<double>& data) {
    if (data.empty()) throw std::invalid_argument("Cannot find min of empty dataset");
    return *std::min_element(data.begin(), data.end());
}

double Statistics::max(const std::vector<double>& data) {
    if (data.empty()) throw std::invalid_argument("Cannot find max of empty dataset");
    return *std::max_element(data.begin(), data.end());
}

double Statistics::percentile(std::vector<double> data, double p) {
    if (data.empty()) throw std::invalid_argument("Cannot compute percentile of empty dataset");
    if (p < 0 || p > 100) throw std::invalid_argument("Percentile must be between 0 and 100");
    std::sort(data.begin(), data.end());
    double index = (p / 100.0) * (data.size() - 1);
    size_t lower = static_cast<size_t>(index);
    double frac = index - lower;
    if (lower + 1 < data.size()) {
        return data[lower] + frac * (data[lower + 1] - data[lower]);
    }
    return data[lower];
}

std::pair<double, double> Statistics::linear_regression(
    const std::vector<double>& x, const std::vector<double>& y) {
    if (x.size() != y.size() || x.size() < 2) {
        throw std::invalid_argument("Need equal-length vectors with at least 2 points");
    }
    double mx = mean(x);
    double my = mean(y);
    double num = 0.0, den = 0.0;
    for (size_t i = 0; i < x.size(); ++i) {
        num += (x[i] - mx) * (y[i] - my);
        den += (x[i] - mx) * (x[i] - mx);
    }
    double slope = num / den;
    double intercept = my - slope * mx;
    return {slope, intercept};
}

} // namespace matheng
