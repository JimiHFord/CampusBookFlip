module.exports = function(options) {
  var RedisStore = require('connect-redis')(options.session);
  return {
    secret: 'jv93n8x9wp9h3h98hqllwekno09j2nds302v8hzq9eg7stwhx54f2mv2bgd02j',
    name: 'com.campusbookflip',
    saveUninitialized: true,
    resave: false,
    store: new RedisStore({
      host: options.host,
      port: options.port
    }),
  };
};
