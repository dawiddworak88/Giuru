import React from "react";
import ReactDOMServer from "react-dom/server";
import { ServerStyleSheets } from "@mui/styles";

import RegisterPage from "../../src/areas/Accounts/pages/Register/RegisterPage";
import ResetPasswordPage from "../../src/areas/Accounts/pages/ResetPassword/ResetPasswordPage";
import SignInPage from "../../src/areas/Accounts/pages/SignIn/SignInPage";
import SetPasswordPage from "../../src/areas/Accounts/pages/SetPassword/SetPasswordPage";
import ContentPage from "../../src/areas/Home/pages/Content/ContentPage";

const Components = {
	RegisterPage,
	ResetPasswordPage,
	SignInPage,
	SetPasswordPage,
	ContentPage
};

const serverRenderer = (req, res, next) => {

	let Component = Components[req.body.moduleName];

	if (Component) {

		const sheets = new ServerStyleSheets();

		ReactDOMServer.renderToString(
			sheets.collect(
				<Component {...req.body.parameters} />
			)
		);

		const css = sheets.toString();

		return res.send(
			ReactDOMServer.renderToString(
				<React.Fragment>
					{css &&
						<style id="jss-server-side">
							{css}
						</style>
					}
					<Component {...req.body.parameters} />
				</React.Fragment>
			));
	}

	res.status(400).end();
};

export default serverRenderer;
