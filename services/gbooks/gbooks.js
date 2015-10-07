var request = require('request'),
    mappings = require('./mappings'),
    config = require('node-yaml-config').load(__dirname + '/config.yaml');

function _randomInt(min, max)
{
  return Math.floor(Math.random() * (max-min+1) + min);
}

function _format(options) {
  var append = '';
  for(var option in options) {
    if(option in mappings) {
      append += '+' + mappings[option] + ':' + options[option];
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
      key: config.apiKeys[_randomInt(0, config.apiKeys.length - 1)]
    }
  };
  request(options, function(error, response, body) {
    callback(error, response, !error && response.statusCode == 200 ?
      JSON.parse(body) : null);
  });
}

module.exports = {
  search: search,
  _format: _format
};
