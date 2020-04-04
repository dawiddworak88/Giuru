import React from 'react'
import ReactDOMServer from 'react-dom/server'

// AspNetCore
import HomePage from '../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage';

// Account
import SignInPage from '../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage';

// Tenant Portal
import OrderPage from '../../src/project/Tenant.Portal/areas/Orders/pages/OrderPage/OrderPage';
import ClientPage from '../../src/project/Tenant.Portal/areas/Clients/pages/ClientPage/ClientPage';
import ClientDetailPage from '../../src/project/Tenant.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage';
import ProductPage from '../../src/project/Tenant.Portal/areas/Products/pages/ProductPage/ProductPage';

const Components = {
	HomePage,
	SignInPage,
	OrderPage,
	ClientPage,
	ClientDetailPage,
	ProductPage
  };

export default (req, res, next) => {

    let Component = Components[req.body.moduleName];
	
	if (Component) {
		return res.send(ReactDOMServer.renderToString(<Component {...req.body.parameters} />));
	}
	
	res.status(400).end();
}