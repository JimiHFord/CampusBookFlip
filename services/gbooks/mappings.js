/*
intitle:  Returns results where the text following this keyword is found in the
          title.
inauthor: Returns results where the text following this keyword is found in the
          author.
inpublisher:  Returns results where the text following this keyword is found in
              the publisher.
subject:  Returns results where the text following this keyword is listed in the
          category list of the volume.
isbn: Returns results where the text following this keyword is the ISBN number.
lccn: Returns results where the text following this keyword is the Library of
      Congress Control Number.
oclc: Returns results where the text following this keyword is the Online
      Computer Library Center number.
*/

module.exports = {
  query: 'q',
  title: 'intitle',
  author: 'inauthor',
  publisher: 'inpublisher',
  subject: 'subject',
  isbn: 'isbn',
  lccn: 'lccn',
  oclc: 'oclc'
};
