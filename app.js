module.exports = function () {
  var yaml_config = require('node-yaml-config');
  var config = yaml_config.load('./config/db.yaml');
  // express app
  var express = require('express');
  var app = express();

  // database config and connection
  var dbConfig = config.database;
  var mongoose = require('mongoose');
  mongoose.connect('mongodb://'+dbConfig.host+'/'+dbConfig.db);

  // register models
  require('./models/initialize')();

  // passport config
  var passport = require('passport');
  // session data
  var session = require('express-session');
  // configure session - reuse mongoose connection for data store
  var sessionConfig = require('./config/session')({
    secret: config.redis.secret,
    session: session
  });


  app.use(session(sessionConfig));
  app.use(function (req, res, next) {
    if (!req.session) {
      return next(new Error('disconnected from redis server')); // handle error
    }
    next(); // otherwise continue
  });
  app.use(passport.initialize());
  app.use(passport.session());


  var path = require('path');
  var favicon = require('serve-favicon');
  var logger = require('morgan');
  var cookieParser = require('cookie-parser');
  var bodyParser = require('body-parser');


  // middleware
  var lessMiddleware = require('less-middleware');





  // view engine setup
  app.set('views', path.join(__dirname, 'views'));
  app.set('view engine', 'hbs');
  var hbs = require('hbs');
  hbs.registerPartials(__dirname + '/views/partials');

  // uncomment after placing your favicon in /public
  app.use(favicon(__dirname + '/public/favicon.ico'));
  app.use(logger('dev'));
  app.use(bodyParser.json());
  app.use(bodyParser.urlencoded({ extended: false }));
  app.use(cookieParser(sessionConfig.secret));
  if (app.get('env') === 'development') {
    app.use(lessMiddleware('/less', {
      dest: '/css',
      pathRoot: path.join(__dirname, 'public')
    }));
  }
  app.use(express.static(path.join(__dirname, 'public')));

  var flash = require('connect-flash');
  app.use(flash(sessionConfig.secret));

  // Init passport
  var initPassport = require('./passport/init');
  initPassport(passport);

  var controllers = require('./controllers/index')(passport);
  var usersRoute = require('./controllers/users');
  var auth = require('./passport/authController');
  var api = require('./controllers/api/api');
  var account = require('./controllers/account');

  app.use('/', controllers);
  app.use('/users', usersRoute());
  app.use('/api', api);
  app.use('/auth', auth.methods(passport));
  app.get('/logout', function(req, res) {
    req.logout();
    res.redirect('/');
  });
  app.use('/account', account);

  // catch 404 and forward to error handler
  app.use(function(req, res, next) {
      var err = new Error('Not Found');
      err.status = 404;
      next(err);
  });

  // error handlers

  // development error handler
  // will print stacktrace
  if (app.get('env') === 'development') {
      app.use(function(err, req, res, next) {
          res.status(err.status || 500);
          res.render('error', {
              message: err.message,
              error: err
          });
      });
  }

  // production error handler
  // no stacktraces leaked to user
  app.use(function(err, req, res, next) {
      res.status(err.status || 500);
      res.render('error', {
          message: err.message,
          error: {}
      });
  });
  return app;
}
