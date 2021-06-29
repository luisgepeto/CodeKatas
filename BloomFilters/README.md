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
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 2707 HashingFunctions: 5
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 3124 HashingFunctions: 6
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 3847 HashingFunctions: 7
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 4239 HashingFunctions: 8
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 4765 HashingFunctions: 9
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 5459 HashingFunctions: 10
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 5939 HashingFunctions: 11
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 6661 HashingFunctions: 12
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 7191 HashingFunctions: 13
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 7558 HashingFunctions: 14
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 8447 HashingFunctions: 15
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 8812 HashingFunctions: 16
FalsePositiveRate: 0% BitArrayLength: 10000000 InitializationMilliseconds: 9323 HashingFunctions: 17
FalsePositiveRate: 1% BitArrayLength: 10000000 InitializationMilliseconds: 141 HashingFunctions: 1
FalsePositiveRate: 2% BitArrayLength: 10000000 InitializationMilliseconds: 1002 HashingFunctions: 2
FalsePositiveRate: 2% BitArrayLength: 10000000 InitializationMilliseconds: 1662 HashingFunctions: 3
FalsePositiveRate: 2% BitArrayLength: 10000000 InitializationMilliseconds: 2275 HashingFunctions: 4
FalsePositiveRate: 21% BitArrayLength: 1000000 InitializationMilliseconds: 997 HashingFunctions: 2
FalsePositiveRate: 21% BitArrayLength: 1000000 InitializationMilliseconds: 1856 HashingFunctions: 3
FalsePositiveRate: 21% BitArrayLength: 1000000 InitializationMilliseconds: 2261 HashingFunctions: 4
FalsePositiveRate: 27% BitArrayLength: 1000000 InitializationMilliseconds: 117 HashingFunctions: 1
FalsePositiveRate: 40% BitArrayLength: 1000000 InitializationMilliseconds: 2659 HashingFunctions: 5
FalsePositiveRate: 42% BitArrayLength: 1000000 InitializationMilliseconds: 3096 HashingFunctions: 6
FalsePositiveRate: 45% BitArrayLength: 1000000 InitializationMilliseconds: 3860 HashingFunctions: 7
FalsePositiveRate: 55% BitArrayLength: 1000000 InitializationMilliseconds: 4233 HashingFunctions: 8
FalsePositiveRate: 70% BitArrayLength: 1000000 InitializationMilliseconds: 4778 HashingFunctions: 9
FalsePositiveRate: 73% BitArrayLength: 1000000 InitializationMilliseconds: 5308 HashingFunctions: 10
FalsePositiveRate: 73% BitArrayLength: 1000000 InitializationMilliseconds: 5997 HashingFunctions: 11
FalsePositiveRate: 74% BitArrayLength: 1000000 InitializationMilliseconds: 6563 HashingFunctions: 12
FalsePositiveRate: 79% BitArrayLength: 1000000 InitializationMilliseconds: 7365 HashingFunctions: 13
FalsePositiveRate: 83% BitArrayLength: 1000000 InitializationMilliseconds: 7534 HashingFunctions: 14
FalsePositiveRate: 87% BitArrayLength: 1000000 InitializationMilliseconds: 8131 HashingFunctions: 15
FalsePositiveRate: 93% BitArrayLength: 1000000 InitializationMilliseconds: 8661 HashingFunctions: 16
FalsePositiveRate: 95% BitArrayLength: 100000 InitializationMilliseconds: 105 HashingFunctions: 1
FalsePositiveRate: 96% BitArrayLength: 1000000 InitializationMilliseconds: 9170 HashingFunctions: 17
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 115 HashingFunctions: 1
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 1003 HashingFunctions: 2
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 1519 HashingFunctions: 3
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 2248 HashingFunctions: 4
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 2642 HashingFunctions: 5
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 3279 HashingFunctions: 6
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 3617 HashingFunctions: 7
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 4212 HashingFunctions: 8
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 4836 HashingFunctions: 9
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 5275 HashingFunctions: 10
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 5901 HashingFunctions: 11
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 6415 HashingFunctions: 12
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 7161 HashingFunctions: 13
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 7563 HashingFunctions: 14
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 8128 HashingFunctions: 15
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 8709 HashingFunctions: 16
FalsePositiveRate: 100% BitArrayLength: 10 InitializationMilliseconds: 9251 HashingFunctions: 17
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 118 HashingFunctions: 1
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 990 HashingFunctions: 2
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 1520 HashingFunctions: 3
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 2362 HashingFunctions: 4
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 2587 HashingFunctions: 5
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 3148 HashingFunctions: 6
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 3622 HashingFunctions: 7
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 4163 HashingFunctions: 8
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 4799 HashingFunctions: 9
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 5303 HashingFunctions: 10
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 5858 HashingFunctions: 11
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 6458 HashingFunctions: 12
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 7260 HashingFunctions: 13
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 7543 HashingFunctions: 14
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 8111 HashingFunctions: 15
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 8642 HashingFunctions: 16
FalsePositiveRate: 100% BitArrayLength: 100 InitializationMilliseconds: 9615 HashingFunctions: 17
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 110 HashingFunctions: 1
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 975 HashingFunctions: 2
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 1592 HashingFunctions: 3
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 2173 HashingFunctions: 4
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 2554 HashingFunctions: 5
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 3113 HashingFunctions: 6
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 3724 HashingFunctions: 7
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 4210 HashingFunctions: 8
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 4796 HashingFunctions: 9
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 5255 HashingFunctions: 10
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 5938 HashingFunctions: 11
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 6379 HashingFunctions: 12
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 7149 HashingFunctions: 13
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 7532 HashingFunctions: 14
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 8066 HashingFunctions: 15
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 8603 HashingFunctions: 16
FalsePositiveRate: 100% BitArrayLength: 1000 InitializationMilliseconds: 9423 HashingFunctions: 17
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 102 HashingFunctions: 1
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 1028 HashingFunctions: 2
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 1551 HashingFunctions: 3
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 2192 HashingFunctions: 4
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 2618 HashingFunctions: 5
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 3082 HashingFunctions: 6
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 3650 HashingFunctions: 7
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 4184 HashingFunctions: 8
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 4925 HashingFunctions: 9
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 5391 HashingFunctions: 10
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 5806 HashingFunctions: 11
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 6505 HashingFunctions: 12
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 7152 HashingFunctions: 13
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 7522 HashingFunctions: 14
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 8044 HashingFunctions: 15
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 8537 HashingFunctions: 16
FalsePositiveRate: 100% BitArrayLength: 10000 InitializationMilliseconds: 9422 HashingFunctions: 17
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 958 HashingFunctions: 2
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 1781 HashingFunctions: 3
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 2199 HashingFunctions: 4
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 2599 HashingFunctions: 5
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 3100 HashingFunctions: 6
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 3734 HashingFunctions: 7
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 4202 HashingFunctions: 8
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 4931 HashingFunctions: 9
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 5280 HashingFunctions: 10
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 5847 HashingFunctions: 11
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 6448 HashingFunctions: 12
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 7473 HashingFunctions: 13
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 7500 HashingFunctions: 14
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 8042 HashingFunctions: 15
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 8502 HashingFunctions: 16
FalsePositiveRate: 100% BitArrayLength: 100000 InitializationMilliseconds: 9256 HashingFunctions: 17