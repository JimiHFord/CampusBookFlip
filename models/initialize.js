var fs = require('fs');

var _ran = false;

module.exports = function() {
  if(!_ran) {
    var models = fs.readdirSync(__dirname);
    for(var i = 0; i < models.length; i++) {
      models[i] !== 'initialize.js' && require('./'+models[i])();
    }
  }
  return _ran = true;
};
