# How to run

Navigate to the SpellChecker.Demo repo and run with the following command 

* For English: `dotnet run -l en -t "text to verify"`
* For Spanish: `dotnet run -l es -t "texto a verificar"`
* To run diagnostics (takes a while) `dotnet run -d`

# [Kata05: Bloom Filters](http://codekata.com/kata/kata05-bloom-filters/)

There are many circumstances where we need to find out if something is a member of a set, and many algorithms for doing it. If the set is small, you can use bitmaps. When they get larger, hashes are a useful technique. But when the sets get big, we start bumping in to limitations. Holding 250,000 words in memory for a spell checker might be too big an overhead if your target environment is a PDA or cell phone. Keeping a list of web-pages visited might be extravagant when you get up to tens of millions of pages. **Fortunately**, there’s a technique that can help.

Bloom filters are a 30-year-old statistical way of testing for membership in a set. They greatly reduce the amount of storage you need to represent the set, but at a price: they’ll sometimes report that something is in the set when it isn’t (but it’ll never do the opposite; if the filter says that the set doesn’t contain your object, you know that it doesn’t). And the nice thing is you can control the accuracy; the more memory you’re prepared to give the algorithm, the fewer false positives you get. I once wrote a spell checker for a PDP-11 which stored a dictionary of 80,000 words in 16kbytes, and I very rarely saw it let though an incorrect word. (Update: I must have mis-remembered these figures, because they are not in line with the theory. Unfortunately, I can no longer read the 8” floppies holding the source, so I can’t get the correct numbers. Let’s just say that I got a decent sized dictionary, along with the spell checker, all in under 64k.)

Bloom filters are very simple. Take a big array of bits, initially all zero. Then take the things you want to look up (in our case we’ll use a dictionary of words). Produce ‘n’ independent hash values for each word. Each hash is a number which is used to set the corresponding bit in the array of bits. Sometimes there’ll be clashes, where the bit will already be set from some other word. This doesn’t matter.

To check to see of a new word is already in the dictionary, perform the same hashes on it that you used to load the bitmap. Then check to see if each of the bits corresponding to these hash values is set. If any bit is not set, then you never loaded that word in, and you can reject it.

The Bloom filter reports a false positive when a set of hashes for a word all end up corresponding to bits that were set previously by other words. In practice this doesn’t happen too often as long as the bitmap isn’t too heavily loaded with one-bits (clearly if every bit is one, then it’ll give a false positive on every lookup). There’s a discussion of the math in Bloom filters at www.cs.wisc.edu/~cao/papers/summary-cache/node8.html.

So, this kata is fairly straightforward. Implement a Bloom filter based spell checker. You’ll need some kind of bitmap, some hash functions, and a simple way of reading in the dictionary and then the words to check. For the hash function, remember that you can always use something that generates a fairly long hash (such as MD5) and then take your smaller hash values by extracting sequences of bits from the result. On a Unix box you can find a list of words in /usr/dict/words (or possibly in /usr/share/dict/words). For others, I’ve put a word list up here.1

Play with using different numbers of hashes, and with different bitmap sizes.

Part two of the exercise is optional. Try generating random 5-character words and feeding them in to your spell checker. For each word that it says it OK, look it up in the original dictionary. See how many false positives you get.

## Analysis Summary

FalsePositiveRate,BitArrayLength,InitializationMilliseconds,HashingFunctions\
0%,5000000,1469,6\
0%,5000000,1522,8\
0%,5000000,1604,7\
0%,5000000,1671,10\
0%,5000000,1797,15\
0%,5000000,1844,17\
0%,5000000,1859,9\
0%,5000000,1984,11\
0%,5000000,1995,14\
0%,5000000,2046,13\
0%,5000000,2057,18\
0%,5000000,2143,12\
0%,5000000,2192,19\
0%,5000000,2403,16\
0%,50000000,1504,8\
0%,50000000,1510,7\
0%,50000000,1522,6\
0%,50000000,1675,5\
0%,50000000,1718,10\
0%,50000000,1900,15\
0%,50000000,1950,9\
0%,50000000,1992,12\
0%,50000000,2023,11\
0%,50000000,2138,17\
0%,50000000,2167,14\
0%,50000000,2190,13\
0%,50000000,2272,19\
0%,50000000,2513,16\
0%,50000000,2569,18\
1%,5000000,1584,5\
86%,500000,1514,5\
92%,500000,1435,6\
94%,500000,1560,7\
98%,500000,1663,8\
98%,500000,1669,9\
99%,500000,1712,10\
99%,500000,2066,12\
99%,500000,2170,13\
100%,500000,1718,14\
100%,500000,1828,17\
100%,500000,1904,15\
100%,500000,1979,11\
100%,500000,1986,19\
100%,500000,2308,16\
100%,500000,2699,18