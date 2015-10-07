module.exports = function(options) {
  var RedisStore = require('connect-redis')(options.session);
  return {
    secret: options.secret,
    name: 'com.campusbookflip',
    saveUninitialized: true,
    resave: false,
    store: new RedisStore({
      host: options.host,
      port: options.port
    }),
  };
};
