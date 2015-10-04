var models = ['Book', 'College', 'User'];

module.exports = function() {
  for(var i = 0; i < models.length; i++) {
    require('./'+models[i])();
  }
};
