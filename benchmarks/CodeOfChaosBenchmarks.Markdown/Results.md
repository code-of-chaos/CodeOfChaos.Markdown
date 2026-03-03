# General Benchmark
| Method                |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |    Gen1 |    Gen2 | Allocated | Alloc Ratio |
|-----------------------|---------:|---------:|---------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| SerializeToSyntaxTree | 522.3 us | 17.91 us | 50.23 us |  1.01 |    0.13 | 38.0859 | 12.6953 |       - | 315.33 KB |        1.00 |
| RenderToHtmlString    | 508.2 us |  9.24 us | 15.93 us |  0.98 |    0.09 | 39.0625 | 16.6016 |       - | 321.38 KB |        1.02 |
| RenderToMarkdown      | 536.9 us | 10.50 us | 12.50 us |  1.04 |    0.09 | 46.8750 |  0.9766 |       - | 390.22 KB |        1.24 |
| RenderToXmlString     | 651.4 us | 12.92 us | 29.42 us |  1.26 |    0.12 | 74.2188 | 35.1563 |       - | 637.47 KB |        2.02 |
| RenderToJsonString    | 771.2 us | 15.18 us | 25.37 us |  1.49 |    0.14 | 94.7266 | 94.7266 | 94.7266 | 789.35 KB |        2.50 |

# Individual Benchmarks

| Method                | InputCase            |         Mean |       Error |      StdDev |       Median |    Gen0 |   Gen1 |   Gen2 | Allocated |
|-----------------------|----------------------|-------------:|------------:|------------:|-------------:|--------:|-------:|-------:|----------:|
| SerializeToSyntaxTree | BlockQuote_100Lines  | 105,226.2 ns | 1,987.14 ns | 2,288.40 ns | 105,432.3 ns | 13.0615 | 5.0049 |      - | 107.17 KB |
| SerializeToSyntaxTree | BlockQuote_10Lines   |  11,268.0 ns |   185.50 ns |   173.51 ns |  11,235.6 ns |  1.5259 | 0.0763 |      - |  12.49 KB |
| SerializeToSyntaxTree | BlockQuote_1Line     |   1,792.2 ns |    25.36 ns |    21.18 ns |   1,788.1 ns |  0.3033 | 0.0038 |      - |   2.48 KB |
| SerializeToSyntaxTree | BlockQuote_2Lines    |   2,918.5 ns |    57.65 ns |    56.62 ns |   2,917.9 ns |  0.4463 | 0.0076 |      - |   3.67 KB |
| SerializeToSyntaxTree | BlockQuote_3Lines    |   3,063.5 ns |    57.92 ns |    59.48 ns |   3,059.4 ns |  0.4539 | 0.0076 |      - |   3.73 KB |
| SerializeToSyntaxTree | Bold                 |   1,558.5 ns |    28.75 ns |    25.49 ns |   1,561.9 ns |  0.3147 | 0.0038 |      - |   2.57 KB |
| SerializeToSyntaxTree | BoldAndItalic        |   2,388.7 ns |    46.79 ns |    48.05 ns |   2,383.1 ns |  0.4120 | 0.0038 |      - |   3.37 KB |
| SerializeToSyntaxTree | BoldA(...)Other [28] |   2,897.7 ns |    56.96 ns |    88.68 ns |   2,891.1 ns |  0.4311 | 0.0076 |      - |   3.53 KB |
| SerializeToSyntaxTree | Bold_2InLine         |   2,622.4 ns |    48.13 ns |    98.32 ns |   2,596.4 ns |  0.4158 | 0.0038 |      - |    3.4 KB |
| SerializeToSyntaxTree | Break                |   1,924.9 ns |    37.88 ns |    53.11 ns |   1,929.9 ns |  0.2899 | 0.0038 |      - |   2.38 KB |
| SerializeToSyntaxTree | Callout              |   2,708.3 ns |    53.62 ns |    61.75 ns |   2,700.6 ns |  0.4463 | 0.0076 |      - |   3.67 KB |
| SerializeToSyntaxTree | Callout_withoutBody  |   1,405.7 ns |    27.94 ns |    29.89 ns |   1,410.4 ns |  0.3185 | 0.0038 |      - |    2.6 KB |
| SerializeToSyntaxTree | CodeBlock            |   2,277.5 ns |    45.06 ns |    61.68 ns |   2,288.4 ns |  0.3090 | 0.0038 |      - |   2.55 KB |
| SerializeToSyntaxTree | CodeBlock_100Lines   |  38,488.2 ns |   748.42 ns | 1,423.94 ns |  38,230.3 ns |  1.5869 | 0.0610 |      - |  13.09 KB |
| SerializeToSyntaxTree | CodeBlock_50Lines    |  21,217.4 ns |   417.53 ns |   674.23 ns |  21,172.7 ns |  0.9155 | 0.0305 |      - |   7.62 KB |
| SerializeToSyntaxTree | CodeBlock_NoLanguage |   1,152.2 ns |    21.94 ns |    24.39 ns |   1,146.5 ns |  0.2670 | 0.0038 |      - |   2.19 KB |
| SerializeToSyntaxTree | CodeInline           |   1,382.6 ns |    26.42 ns |    30.43 ns |   1,379.4 ns |  0.3014 | 0.0038 |      - |   2.47 KB |
| SerializeToSyntaxTree | CodeInline_2ticks    |   1,407.0 ns |    28.12 ns |    37.54 ns |   1,418.8 ns |  0.3014 | 0.0038 |      - |   2.47 KB |
| SerializeToSyntaxTree | CodeInline_3ticks    |   1,489.5 ns |    27.93 ns |    37.29 ns |   1,495.9 ns |  0.3014 | 0.0038 |      - |   2.47 KB |
| SerializeToSyntaxTree | Emote                |   1,291.8 ns |    25.68 ns |    49.47 ns |   1,288.3 ns |  0.2365 | 0.0019 |      - |   1.94 KB |
| SerializeToSyntaxTree | EscapedCharacters    |   1,985.8 ns |    39.36 ns |    76.77 ns |   1,989.2 ns |  0.3128 | 0.0038 |      - |   2.57 KB |
| SerializeToSyntaxTree | FootnoteDescription  |   1,788.4 ns |    33.46 ns |    59.47 ns |   1,778.9 ns |  0.3357 | 0.0038 |      - |   2.75 KB |
| SerializeToSyntaxTree | FootnoteReference    |   1,247.4 ns |    24.37 ns |    37.22 ns |   1,252.1 ns |  0.2842 | 0.0038 |      - |   2.34 KB |
| SerializeToSyntaxTree | FrontMatter          |     707.3 ns |    14.06 ns |    13.15 ns |     710.5 ns |  0.2518 | 0.0010 |      - |   2.06 KB |
| SerializeToSyntaxTree | FrontMatter_2Entries |   3,523.7 ns |    62.55 ns |    79.11 ns |   3,553.1 ns |  0.5035 | 0.0114 |      - |   4.13 KB |
| SerializeToSyntaxTree | HeadingSimple        |   1,015.3 ns |    11.20 ns |     8.75 ns |   1,018.9 ns |  0.2661 | 0.0038 |      - |   2.18 KB |
| SerializeToSyntaxTree | Heading_1            |     938.9 ns |    16.87 ns |    18.75 ns |     944.1 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Heading_2            |     930.8 ns |    15.43 ns |    13.68 ns |     930.0 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Heading_3            |     946.8 ns |    18.95 ns |    16.80 ns |     943.4 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Heading_4            |     931.0 ns |    17.63 ns |    15.63 ns |     928.6 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Heading_5            |     952.4 ns |    18.82 ns |    32.46 ns |     945.7 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Heading_6            |     938.9 ns |    16.61 ns |    14.72 ns |     940.9 ns |  0.2565 | 0.0029 |      - |    2.1 KB |
| SerializeToSyntaxTree | Highlight            |   1,779.6 ns |    33.07 ns |    29.31 ns |   1,779.9 ns |  0.3166 | 0.0038 |      - |   2.59 KB |
| SerializeToSyntaxTree | HorizontalRule       |     690.1 ns |    12.55 ns |    13.43 ns |     691.5 ns |  0.2089 | 0.0019 |      - |   1.71 KB |
| SerializeToSyntaxTree | HtmlBlock            |   1,952.3 ns |    38.72 ns |    75.52 ns |   1,962.1 ns |  0.3262 | 0.0038 |      - |   2.66 KB |
| SerializeToSyntaxTree | Italic               |   1,651.8 ns |    32.13 ns |    30.05 ns |   1,647.3 ns |  0.3300 | 0.0038 |      - |    2.7 KB |
| SerializeToSyntaxTree | Italic_2InLine       |   2,659.1 ns |    51.96 ns |    53.36 ns |   2,671.0 ns |  0.4501 | 0.0076 |      - |    3.7 KB |
| SerializeToSyntaxTree | Link                 |   2,374.8 ns |    47.40 ns |    79.19 ns |   2,386.1 ns |  0.3700 | 0.0038 |      - |   3.02 KB |
| SerializeToSyntaxTree | Link_Nested          |   3,483.9 ns |    67.31 ns |   110.60 ns |   3,475.6 ns |  0.3624 | 0.0038 |      - |   2.97 KB |
| SerializeToSyntaxTree | List_Ordered         |   2,431.4 ns |    44.93 ns |    86.56 ns |   2,419.0 ns |  0.5684 | 0.0153 |      - |   4.67 KB |
| SerializeToSyntaxTree | List_Ordered_100     |  78,711.6 ns | 1,568.98 ns | 1,867.76 ns |  78,438.0 ns | 17.0898 | 8.3008 |      - | 139.82 KB |
| SerializeToSyntaxTree | List_Ordered_50      |  39,046.4 ns |   749.58 ns |   802.04 ns |  38,983.5 ns |  8.6060 | 2.1973 |      - |  70.49 KB |
| SerializeToSyntaxTree | List_Task            |   1,661.8 ns |    23.86 ns |    21.16 ns |   1,667.5 ns |  0.3986 | 0.0076 |      - |   3.27 KB |
| SerializeToSyntaxTree | List_Task_100        |  76,508.1 ns | 1,416.66 ns | 1,325.14 ns |  76,554.0 ns | 17.2119 | 7.9346 |      - | 140.99 KB |
| SerializeToSyntaxTree | List_Task_50         |  39,090.4 ns |   689.29 ns |   943.51 ns |  39,152.0 ns |  8.6670 | 2.3193 |      - |  71.07 KB |
| SerializeToSyntaxTree | List_Task_checked    |   1,652.9 ns |    32.62 ns |    68.08 ns |   1,654.2 ns |  0.3986 | 0.0076 |      - |   3.27 KB |
| SerializeToSyntaxTree | List_(...)d_100 [21] |  77,882.5 ns | 1,524.09 ns | 2,372.82 ns |  77,696.3 ns | 17.2119 | 7.9346 |      - | 140.99 KB |
| SerializeToSyntaxTree | List_Task_checked_50 |  38,782.3 ns |   771.09 ns | 1,154.14 ns |  38,779.1 ns |  8.6670 | 2.3193 |      - |  71.07 KB |
| SerializeToSyntaxTree | List_UnOrdered       |   2,391.2 ns |    46.14 ns |    67.64 ns |   2,389.5 ns |  0.5569 | 0.0153 |      - |   4.56 KB |
| SerializeToSyntaxTree | List_UnOrdered_100   |  77,688.8 ns | 1,514.74 ns | 2,172.39 ns |  78,003.9 ns | 16.3574 | 7.6904 |      - | 133.96 KB |
| SerializeToSyntaxTree | List_UnOrdered_50    |  38,445.4 ns |   766.52 ns |   912.49 ns |  38,383.3 ns |  8.2397 | 2.0752 |      - |  67.56 KB |
| SerializeToSyntaxTree | Mixed_RealWorld      |  17,099.1 ns |   333.10 ns |   356.41 ns |  17,089.1 ns |  2.1667 | 0.1221 |      - |  17.76 KB |
| SerializeToSyntaxTree | NewLine              |   2,251.8 ns |    44.85 ns |    88.54 ns |   2,276.1 ns |  0.3700 | 0.0038 |      - |   3.03 KB |
| SerializeToSyntaxTree | Paragraph            |   1,343.3 ns |    26.39 ns |    35.22 ns |   1,344.8 ns |  0.2365 | 0.0019 |      - |   1.94 KB |
| SerializeToSyntaxTree | Paragraph_Base       |   1,254.3 ns |    25.08 ns |    38.31 ns |   1,267.6 ns |  0.2365 | 0.0019 |      - |   1.94 KB |
| SerializeToSyntaxTree | Strikethrough        |   1,820.0 ns |    27.35 ns |    24.24 ns |   1,816.7 ns |  0.3166 | 0.0038 |      - |   2.59 KB |
| SerializeToSyntaxTree | Strik(...)nLine [21] |   3,021.0 ns |    57.87 ns |   104.36 ns |   3,003.1 ns |  0.4158 | 0.0076 |      - |   3.41 KB |
| SerializeToSyntaxTree | Subscript            |   1,679.8 ns |    32.09 ns |    41.72 ns |   1,679.7 ns |  0.3300 | 0.0038 |      - |    2.7 KB |
| SerializeToSyntaxTree | Subscript_2InLine    |   2,741.0 ns |    54.45 ns |    66.87 ns |   2,732.8 ns |  0.4501 | 0.0076 |      - |    3.7 KB |
| SerializeToSyntaxTree | Superscript          |   1,646.7 ns |    32.67 ns |    37.62 ns |   1,634.5 ns |  0.3300 | 0.0038 |      - |    2.7 KB |
| SerializeToSyntaxTree | Superscript_2InLine  |   2,746.5 ns |    54.55 ns |    88.09 ns |   2,742.2 ns |  0.4501 | 0.0076 |      - |    3.7 KB |
| SerializeToSyntaxTree | Table                |   3,474.4 ns |    61.82 ns |    66.14 ns |   3,463.7 ns |  0.5035 | 0.0114 |      - |   4.13 KB |
| SerializeToSyntaxTree | Table_100Rows        |  79,780.1 ns | 1,536.97 ns | 1,769.98 ns |  79,565.7 ns | 12.4512 | 2.3193 | 1.0986 | 102.68 KB |
| SerializeToSyntaxTree | Table_2Rows          |   4,738.9 ns |    91.72 ns |   112.64 ns |   4,757.5 ns |  0.6256 | 0.0153 |      - |   5.13 KB |
| SerializeToSyntaxTree | Table_3Rows          |   5,850.5 ns |   114.93 ns |   101.89 ns |   5,827.1 ns |  0.7477 | 0.0229 |      - |   6.12 KB |
| SerializeToSyntaxTree | Table_50Rows         |  45,031.0 ns |   840.00 ns | 1,231.26 ns |  45,095.0 ns |  6.4697 | 0.6104 |      - |  53.04 KB |
| SerializeToSyntaxTree | Tag                  |   1,216.8 ns |    22.66 ns |    21.20 ns |   1,218.6 ns |  0.2861 | 0.0038 |      - |   2.34 KB |
| SerializeToSyntaxTree | Template             |   1,890.2 ns |    50.03 ns |   147.53 ns |   1,933.3 ns |  0.3090 | 0.0038 |      - |   2.53 KB |
| SerializeToSyntaxTree | Underline            |   1,638.5 ns |    32.74 ns |    71.18 ns |   1,619.4 ns |  0.3300 | 0.0038 |      - |    2.7 KB |
| SerializeToSyntaxTree | User                 |   1,137.3 ns |    22.21 ns |    22.81 ns |   1,135.2 ns |  0.2861 | 0.0038 |      - |   2.34 KB |
| SerializeToSyntaxTree | WikiLink             |   1,308.0 ns |    26.01 ns |    55.98 ns |   1,296.3 ns |  0.2861 | 0.0038 |      - |   2.35 KB |
| SerializeToSyntaxTree | Wrapper              |   1,852.7 ns |    31.17 ns |    34.65 ns |   1,859.0 ns |  0.3662 | 0.0057 |      - |      3 KB |
