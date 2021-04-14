import express from "express";
import serverRenderer from "./middleware/renderer";

const path = require("path");

const PORT = 3000;

const app = express();

app.use(express.static(path.resolve(__dirname, "public")));
app.use(express.json({limit: "50mb"}));
app.use(express.urlencoded({limit: "50mb", extended: true, parameterLimit:50000}));

const router = express.Router();

router.use("^/$", serverRenderer);

router.use(express.static(
    path.resolve(__dirname, "..", "build"),
    { maxAge: "30d" },
));

app.use(router);

app.listen(PORT, (error) => {
    if (error) {
        return console.log("something bad happened", error);
    }

    console.log("listening on " + PORT + "...");
});
