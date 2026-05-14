import React from "react";
import ReactDOMServer from "react-dom/server";
import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import createEmotionServer from "@emotion/server/create-instance";

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

		const cache = createCache({ key: "css" });
		const { extractCriticalToChunks, constructStyleTagsFromChunks } = createEmotionServer(cache);

		const html = ReactDOMServer.renderToString(
			<CacheProvider value={cache}>
				<Component {...req.body.parameters} />
			</CacheProvider>
		);

		const emotionChunks = extractCriticalToChunks(html);
		const emotionCss = constructStyleTagsFromChunks(emotionChunks);

		return res.send(emotionCss + html);
	}

	res.status(400).end();
};

export default serverRenderer;