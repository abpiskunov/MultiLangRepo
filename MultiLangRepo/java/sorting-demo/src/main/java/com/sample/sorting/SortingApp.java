package com.sample.sorting;

import java.util.Arrays;
import java.util.Random;
import java.util.function.Consumer;

public class SortingApp {
    public static void main(String[] args) {
        System.out.println("=== Sorting Algorithms Demo ===\n");

        // Small array demo
        Integer[] sample = {64, 34, 25, 12, 22, 11, 90};
        System.out.println("Original: " + Arrays.toString(sample));

        Integer[] arr1 = sample.clone();
        SortAlgorithms.bubbleSort(arr1);
        System.out.println("Bubble:   " + Arrays.toString(arr1));

        Integer[] arr2 = sample.clone();
        SortAlgorithms.selectionSort(arr2);
        System.out.println("Selection:" + Arrays.toString(arr2));

        Integer[] arr3 = sample.clone();
        SortAlgorithms.insertionSort(arr3);
        System.out.println("Insertion:" + Arrays.toString(arr3));

        Integer[] arr4 = sample.clone();
        SortAlgorithms.quickSort(arr4);
        System.out.println("Quick:    " + Arrays.toString(arr4));

        Integer[] arr5 = sample.clone();
        SortAlgorithms.mergeSort(arr5);
        System.out.println("Merge:    " + Arrays.toString(arr5));

        // Benchmark with larger arrays
        System.out.println("\n--- Benchmark (10,000 elements) ---");
        int size = 10_000;
        Integer[] baseArray = generateRandomArray(size);

        benchmark("Bubble Sort",    baseArray, SortAlgorithms::bubbleSort);
        benchmark("Selection Sort", baseArray, SortAlgorithms::selectionSort);
        benchmark("Insertion Sort", baseArray, SortAlgorithms::insertionSort);
        benchmark("Quick Sort",     baseArray, SortAlgorithms::quickSort);
        benchmark("Merge Sort",     baseArray, SortAlgorithms::mergeSort);

        // String sorting
        System.out.println("\n--- String Sorting ---");
        String[] words = {"banana", "apple", "cherry", "date", "elderberry", "fig", "grape"};
        System.out.println("Original: " + Arrays.toString(words));
        SortAlgorithms.quickSort(words);
        System.out.println("Sorted:   " + Arrays.toString(words));
    }

    private static void benchmark(String name, Integer[] base, Consumer<Integer[]> sortFunction) {
        Integer[] copy = base.clone();
        long start = System.nanoTime();
        sortFunction.accept(copy);
        long elapsed = System.nanoTime() - start;

        // Verify sorted
        boolean sorted = true;
        for (int i = 1; i < copy.length; i++) {
            if (copy[i] < copy[i - 1]) {
                sorted = false;
                break;
            }
        }

        System.out.printf("  %-16s %8.2f ms  %s%n",
                name, elapsed / 1_000_000.0, sorted ? "✓" : "✗ NOT SORTED");
    }

    private static Integer[] generateRandomArray(int size) {
        Random random = new Random(42); // fixed seed for reproducibility
        Integer[] arr = new Integer[size];
        for (int i = 0; i < size; i++) {
            arr[i] = random.nextInt(100_000);
        }
        return arr;
    }
}
