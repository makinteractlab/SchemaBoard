var http = require('http');
var express = require('express');
var bodyParser = require('body-parser');
var fs = require('fs');
const path = require('path');


// Settings
// const SERIAL_PORT = 'COM11';
const SERIAL_PORT = '/dev/ttyACM0';


// No need to change this
const SerialPort = require('serialport')
const Readline = require('@serialport/parser-readline')
const port = new SerialPort(SERIAL_PORT, { baudRate: 115200 })

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

// app.post('/get', function(req, res) {
//     if (req.body.cmd == "getFile")
//     {
//     	var fileName= path.join("data", req.body.name+".xml");
//     	fs.readFile(fileName, 'utf8', function(err, contents) {
// 		    res.send (contents);
// 		});
//     }  
// })


app.post('/get', function(req, res) {
    if (req.body.cmd == "getFile")
    {
        var fileName= path.join(__dirname, "data", req.body.name);
        // console.log(fileName);
        fs.readFile(fileName, 'utf8', function(err, contents) {
            console.log(contents);
            // res.send (contents+"\n");
            res.sendFile(fileName);

        });
    }  
})


var server = app.listen(8081, function() {
    var host = server.address().address
    var port = server.address().port
    console.log("Example app listening at http://%s:%s", host, port)
});


