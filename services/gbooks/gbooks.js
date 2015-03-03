var request = require('request'),
    config = require('./config');

function _format(options) {
  var append = '';
  for(var option in options) {
    if(option in config) {
      append += '+' + config[option] + ':' + options[option];
    }
  }
  return append;
}

function search(query, options, callback) {
  var furtherOptions = _format(options);

  options = {
    url: config.baseUrl,
    qs: {
      q: query + furtherOptions,
      key: config.apiKey
    }
  };
  // console.log(options);
  request(options, function(error, response, body) {
    callback(error, response, !error && response.statusCode == 200 ?
      JSON.parse(body) : null);
  });
}

module.exports = {
  search: search,
  _format: _format
};
