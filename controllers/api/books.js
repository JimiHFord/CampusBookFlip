var express = require('express'),
    router = express.Router(),
    gbooks = require('../../services/gbooks/gbooks');

// TODO: fix error handling - it's not working
function handleQuery(req, res, next) {
  var query = req.param('query', 'null');
  gbooks.search(query, req.body, function(err, response, data) {
    if(!err && response.statusCode == 200) {
      res.json(data);
    } else {
      // console.log(err, response);
      next(err);
    }
  });
}


router.get('/search/:query?', handleQuery);


module.exports = router;
