const express = require("express");
const fs = require("fs");


const app = express();

app.set("port", process.env.PORT || 3001);

// Express only serves static assets in production
if (process.env.NODE_ENV === "production") {
  app.use(express.static("client/build"));
}


app.get("/api/stories", (req, res) => {
  var dirCont = fs.readdirSync( '../compiledStories' );
  var files = dirCont.filter(dirCont => '*.json');

  if (files) {
    res.json(
      files
    );
  } else {
    res.json([]);
  }
});

app.get("/api/story/:storyName", (req, res) => {

    var storyName = req.params.storyName;
    console.log(storyName);
    var storyContent = fs.readFileSync('../compiledStories/'+storyName);
    if (storyContent) {
      res.send(
        storyContent
      );
    } else {
      res.json(req.params);
    }
  });

app.listen(app.get("port"), () => {
  console.log(`Find the server at: http://localhost:${app.get("port")}/`); // eslint-disable-line no-console
});