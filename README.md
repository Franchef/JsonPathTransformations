# JsonPathTransformations
A library to transform JSON document with JSONPath


I will take a look here: https://github.com/json-path/JsonPath

## Example

```json
{
  "book" : {
    "title" : "Book title",
    "author" : "who writed the book"
  }
}
```
_transform_
source: `$.book.author` -> destination `$.author`

```json
{
  "author" : "who writed the book"
}
```
