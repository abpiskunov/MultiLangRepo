#pragma once
#include <vector>
#include <cmath>

namespace matheng {

class Statistics {
public:
    static double mean(const std::vector<double>& data);
    static double median(std::vector<double> data);
    static double variance(const std::vector<double>& data);
    static double standard_deviation(const std::vector<double>& data);
    static double min(const std::vector<double>& data);
    static double max(const std::vector<double>& data);
    static double percentile(std::vector<double> data, double p);
    static std::pair<double, double> linear_regression(
        const std::vector<double>& x, const std::vector<double>& y);
};

} // namespace matheng
