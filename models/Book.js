var mongoose = require('mongoose');
var model;

try {
  model = mongoose.model('Book');
} catch (err) {
  model = mongoose.model('Book', {
    id: String,
    etag: String,
    selfLink: String,
    volumeInfo: {
      title: String,
      subtitle: String,
      authors: [String],
      publisher: String,
      publishedDate: String,
      description: String,
      industryIdentifiers: [{
        type: { type: String },
        identifier: String
      }],
      readingModes: {
        text: Boolean,
        image: Boolean
      },
      pageCount: Number,
      printedPageCount: Number,
      printType: String,
      categories: [String],
      averageRating: Number,
      ratingsCount: Number,
      imageLinks: {
        smallThumbnail: String,
        thumbnail: String,
        small: String,
        medium: String,
        large: String,
        extraLarge: String
      },
      language: String,
      previewLink: String,
      infoLink: String,
      canonicalVolumeLink: String
    },
    saleInfo: {
      country: String,
      saleability: String,
      isEbook: Boolean,
      listPrice: {
        amount: Number,
        currencyCode: String
      },
      retailPrice: {
        amount: Number,
        currencyCode: String
      },
      buyLink: String,
      offers: [{
        finskyOfferType: Number,
        listPrice: {
          amountInMicros: Number,
          currencyCode: String
        },
        retailPrice: {
          amountInMicros: Number,
          currencyCode: String
        }
      }]
    },
    accessInfo: {
      country: String,
      viewability: String,
      embeddable: Boolean,
      publicDomain: Boolean,
      textToSpeechPermission: String,
      epub: {
        isAvailable: Boolean,
        acsTokenLink: String
      },
      pdf: {
        isAvailable: Boolean,
        acsTokenLink: String
      },
      webReaderLink: String,
      accessViewStatus: String,
      quoteSharingAllowed: Boolean
    },
    searchInfo: {
      textSnippet: String
    },

  });
}

module.exports = model;
