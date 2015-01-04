var mongoose = require('mongoose');
var model;

try {
  model = mongoose.model('Book');
} catch (err) {
  model = mongoose.model('Book', {
    g_id: String,
    g_etag: String,
    selfLink: String,
    volumeInfo: {
      title: String,
      subtitle: String,
      authors: [String]
    },
    publisher: String,
    publishedDate: String,
    industryIdentifiers: [{
      type: String,
      identifier: String
    }],
    readingModes: {
      text: Boolean,
      image: Boolean
    },
    pageCount: Number,
    printedPageCount: Number,
    printType: String,
    imageLinks: {
      smallThumbnail: String,
      thumbnail: String,
      small: String,
      medium: String,
      large: String,
      extraLarge: String
    },
    language: String,

  });
}

module.exports = model;
