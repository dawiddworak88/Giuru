import React from 'react'
import ReactDOMServer from 'react-dom/server'

// AspNetCore
import HomePage from '../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage';

// Account
import SignInPage from '../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage';

const Components = {
	HomePage,
	SignInPage
  };

export default (req, res, next) => {

    let Component = Components[req.body.moduleName];
	
	if (Component) {
		return res.send(ReactDOMServer.renderToString(<Component {...req.body.parameters} />));
	}
	
	res.status(400).end();
}