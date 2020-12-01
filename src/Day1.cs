
using System;
using System.Linq;

namespace advent_of_code_2020
{
    class Day1
    {
        public Day1()
        {
            Console.WriteLine("Day 1");
            Console.WriteLine("Answer for Part A is {0}", partA());
            Console.WriteLine("Answer for Part B is {0}", partB());
        }

        /** --- Day 1: Report Repair ---

        After saving Christmas five years in a row, you've decided to take a vacation at a nice resort on a tropical island. Surely, Christmas will go on without you.

        The tropical island has its own currency and is entirely cash-only. The gold coins used there have a little picture of a starfish; the locals just call them stars. 
        None of the currency exchanges seem to have heard of them, but somehow, you'll need to find fifty of these coins by the time you arrive so you can pay the deposit on your room.
        To save your vacation, you need to get all fifty stars by December 25th.

        Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

        Before you leave, the Elves in accounting just need you to fix your expense report (your puzzle input); apparently, something isn't quite adding up.

        Specifically, they need you to find the two entries that sum to 2020 and then multiply those two numbers together.

        For example, suppose your expense report contained the following:

        1721
        979
        366
        299
        675
        1456
        
        In this list, the two entries that sum to 2020 are 1721 and 299. Multiplying them together produces 1721 * 299 = 514579, so the correct answer is 514579.

        Of course, your expense report is much larger. Find the two entries that sum to 2020; what do you get if you multiply them together?
        */
        private int partA()
        {
            int[] arr = input.Split('\n').Select(n => Convert.ToInt32(n)).ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[i] + arr[j] == 2020)
                    {
                        return arr[i] * arr[j];
                    }
                }
            }
            // Nothing is found
            return -1;
        }

        /* --- Part Two ---
        The Elves in accounting are thankful for your help; one of them even offers you a starfish coin they had left over from a past vacation. 
        They offer you a second one if you can find three numbers in your expense report that meet the same criteria.
    
        Using the above example again, the three entries that sum to 2020 are 979, 366, and 675. Multiplying them together produces the answer, 241861950.
    
        In your expense report, what is the product of the three entries that sum to 2020?
        */
        private int partB()
        {
            int[] arr = input.Split('\n').Select(n => Convert.ToInt32(n)).ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    for (int k = 0; k < arr.Length; k++)
                    {
                        if (arr[i] + arr[j] + arr[k] == 2020)
                        {
                            return arr[i] * arr[j] * arr[k];
                        }
                    }
                }
            }
            // Nothing is found
            return -1;
        }

        private string input = @"1446
1893
1827
1565
1728
497
1406
1960
1986
1945
1731
1925
1550
1841
1789
1952
1610
1601
1776
1808
1812
1834
1454
1729
513
1894
1703
1587
1788
1690
1655
1473
1822
1437
1626
1447
1400
1396
1715
1720
1469
1388
1874
1641
518
1664
1552
1800
512
1506
1806
1857
1802
1843
1956
1678
1560
1971
1940
1847
1902
1500
1383
1386
1398
1535
1713
1931
1619
1519
1897
1767
1548
1976
1984
1426
914
2000
1585
1634
1832
1849
1665
1609
1670
1520
1490
1746
1608
1829
1431
1762
1384
1504
1434
1356
1654
1719
1599
1686
1489
1377
1531
1912
144
1875
1532
1439
1482
1420
1529
1554
1826
1546
1589
1993
1518
1708
1733
1876
1953
1741
1689
773
1455
1613
2004
1819
1725
1617
1498
1651
2007
1402
728
1475
1928
1904
1969
1851
1296
1558
1817
1663
1750
1780
1501
1443
1734
1977
1901
1547
1631
1644
1815
1949
1586
1697
1435
1783
1772
1987
1483
1372
1999
1848
1512
1541
1861
2008
1607
1622
1629
1763
1656
1661
1581
1968
1985
1974
1882
995
1704
1896
1611
1888
1773
1810
1650
1712
1410
1796
1691
1671
1947
1775
1593
656
1530
1743";

    }
}