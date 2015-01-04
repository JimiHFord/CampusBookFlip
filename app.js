module.exports = function () {

    // express app
    var express = require('express');
    var app = express();

    // database config
    var dbConfig = require('./config/db');
    var mongoose = require('mongoose');
    mongoose.connect(dbConfig.url);


    // passport config
    var passport = require('passport');
    var expressSession = require('express-session');
    app.use(expressSession({ secret: 'com.campusbookflip.expressSession' }));
    app.use(passport.initialize());
    app.use(passport.session());
    /*var User = require('./models/User');
    passport.serializeUser(function(user, done) {
        done(null, user._id);
    });

    passport.deserializeUser(function(id, done) {
        User.findById(id, function(err, user) {
            done(err, user);
        });
    });*/

    var path = require('path');
    var favicon = require('serve-favicon');
    var logger = require('morgan');
    var cookieParser = require('cookie-parser');
    var bodyParser = require('body-parser');


    //var users = require('./controllers/users');





    // view engine setup
    app.set('views', path.join(__dirname, 'views'));
    app.set('view engine', 'hbs');

    // uncomment after placing your favicon in /public
    //app.use(favicon(__dirname + '/public/favicon.ico'));
    app.use(logger('dev'));
    app.use(bodyParser.json());
    app.use(bodyParser.urlencoded({ extended: false }));
    app.use(cookieParser());
    app.use(express.static(path.join(__dirname, 'public')));

    var flash = require('connect-flash');
    app.use(flash());

    // Init passport
    var initPassport = require('./passport/init');
    initPassport(passport);

    var controllers = require('./controllers/index')(passport);
    var usersRoute = require('./controllers/users');
    var api = require('./controllers/api/api');

    app.use('/', controllers);
    app.use('/users', usersRoute());
    app.use('/api', api);

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
