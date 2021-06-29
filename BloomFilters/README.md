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
FalsePositiveRate,BitArrayLength,InitializationMilliseconds,HashingFunctions
0%,10000000,2707,5\
0%,10000000,3124,6\
0%,10000000,3847,7\
0%,10000000,4239,8\
0%,10000000,4765,9\
0%,10000000,5459,10\
0%,10000000,5939,11\
0%,10000000,6661,12\
0%,10000000,7191,13\
0%,10000000,7558,14\
0%,10000000,8447,15\
0%,10000000,8812,16\
0%,10000000,9323,17\
1%,10000000,141,1\
2%,10000000,1002,2\
2%,10000000,1662,3\
2%,10000000,2275,4\
21%,1000000,997,2\
21%,1000000,1856,3\
21%,1000000,2261,4\
27%,1000000,117,1\
40%,1000000,2659,5\
42%,1000000,3096,6\
45%,1000000,3860,7\
55%,1000000,4233,8\
70%,1000000,4778,9\
73%,1000000,5308,10\
73%,1000000,5997,11\
74%,1000000,6563,12\
79%,1000000,7365,13\
83%,1000000,7534,14\
87%,1000000,8131,15\
93%,1000000,8661,16\
95%,100000,105,1\
96%,1000000,9170,17\
100%,10,115,1\
100%,10,1003,2\
100%,10,1519,3\
100%,10,2248,4\
100%,10,2642,5\
100%,10,3279,6\
100%,10,3617,7\
100%,10,4212,8\
100%,10,4836,9\
100%,10,5275,10\
100%,10,5901,11\
100%,10,6415,12\
100%,10,7161,13\
100%,10,7563,14\
100%,10,8128,15\
100%,10,8709,16\
100%,10,9251,17\
100%,100,118,1\
100%,100,990,2\
100%,100,1520,3\
100%,100,2362,4\
100%,100,2587,5\
100%,100,3148,6\
100%,100,3622,7\
100%,100,4163,8\
100%,100,4799,9\
100%,100,5303,10\
100%,100,5858,11\
100%,100,6458,12\
100%,100,7260,13\
100%,100,7543,14\
100%,100,8111,15\
100%,100,8642,16\
100%,100,9615,17\
100%,1000,110,1\
100%,1000,975,2\
100%,1000,1592,3\
100%,1000,2173,4\
100%,1000,2554,5\
100%,1000,3113,6\
100%,1000,3724,7\
100%,1000,4210,8\
100%,1000,4796,9\
100%,1000,5255,10\
100%,1000,5938,11\
100%,1000,6379,12\
100%,1000,7149,13\
100%,1000,7532,14\
100%,1000,8066,15\
100%,1000,8603,16\
100%,1000,9423,17\
100%,10000,102,1\
100%,10000,1028,2\
100%,10000,1551,3\
100%,10000,2192,4\
100%,10000,2618,5\
100%,10000,3082,6\
100%,10000,3650,7\
100%,10000,4184,8\
100%,10000,4925,9\
100%,10000,5391,10\
100%,10000,5806,11\
100%,10000,6505,12\
100%,10000,7152,13\
100%,10000,7522,14\
100%,10000,8044,15\
100%,10000,8537,16\
100%,10000,9422,17\
100%,100000,958,2\
100%,100000,1781,3\
100%,100000,2199,4\
100%,100000,2599,5\
100%,100000,3100,6\
100%,100000,3734,7\
100%,100000,4202,8\
100%,100000,4931,9\
100%,100000,5280,10\
100%,100000,5847,11\
100%,100000,6448,12\
100%,100000,7473,13\
100%,100000,7500,14\
100%,100000,8042,15\
100%,100000,8502,16\
100%,100000,9256,17