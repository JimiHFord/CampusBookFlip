var express = require('express'),
    router = express.Router(),
    mongoose = require('mongoose'),
    College = mongoose.model('College'),
    _ = require('underscore');

// parent module uses "/colleges" for route

function findByCaseInsensitiveName(name, callback) {
  College.find({
    Institution_Name: {
      $regex: '^' + name,
      $options: 'i'
    }
  }, callback);
};

router.get('/named/:name?', function(req, res) {
  var name = req.param('name', null);
  if(!name) {
    var msg = "\'name\' parameter required";
    return res.json({
      message: msg
    });
  }
  findByCaseInsensitiveName(name, function(err, colleges) {
    if(err) {
      throw err;
    }
    var result = _.map(colleges, function(obj) {
      return {
        Institution_Name: obj.Institution_Name,
        Institution_ID: obj.Institution_ID
      };
    }); // result
    res.json(result);
  });
});

module.exports = router;
