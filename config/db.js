var uriUtil = require('mongodb-uri');

module.exports = {
	// 'url': 'mongodb://localhost/cbf-dev'
	'uri': uriUtil.formatMongoose('mongodb://test:Z9zillow369@ds061548.mongolab.com:61548/cbf-prod')
}
