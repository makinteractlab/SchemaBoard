var http = require('http');
var express = require('express');
var bodyParser = require('body-parser');


const SerialPort = require('serialport')
const Readline = require('@serialport/parser-readline')
const port = new SerialPort('/dev/tty.usbmodem14201', { baudRate: 115200 })

const parser = new Readline()
port.pipe(parser)

parser.on('data', line => console.log(`> ${line}`))




var app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));



app.post('/set', function(req, res) {
    var body = JSON.stringify(req.body) + "\n";
    console.log(body);
    port.write(body)
    res.send ("ok");
})

var server = app.listen(8081, function() {
    var host = server.address().address
    var port = server.address().port
    console.log("Example app listening at http://%s:%s", host, port)
});


