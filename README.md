Proof of concept of a simple web api working with multiple caching layers

# Layers of cache
## Memory cache
Implemented using .net IMemory cache. Data will be cached and this will be used as the first layer

## Redis
Using local redis and the .net Redis packages to store data on the second layer.
This implementation will be called if them data in the memory cache is not present

## Source
Currently returns a hard coded object, but should be the source (ex: database, CMS) for getting the data.

# Abstractions
Exposes a few interfaces to implement the services and caching layers

# ICacheSerivce
Interface to build concrete caching services against (redis or memory cache for example)

# ICacheEntry
Interface to expose a property for setting a cache key format.
