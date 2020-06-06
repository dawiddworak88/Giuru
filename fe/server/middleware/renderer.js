import React from 'react';
import ReactDOMServer from 'react-dom/server';
import { ServerStyleSheets } from '@material-ui/core/styles';

// AspNetCore
import HomePage from '../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage';

// Account
import SignInPage from '../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage';

// Tenant Portal
import OrderPage from '../../src/project/Tenant.Portal/areas/Orders/pages/OrderPage/OrderPage';
import ClientPage from '../../src/project/Tenant.Portal/areas/Clients/pages/ClientPage/ClientPage';
import ClientDetailPage from '../../src/project/Tenant.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage';
import ProductPage from '../../src/project/Tenant.Portal/areas/Products/pages/ProductPage/ProductPage';
import ProductDetailPage from '../../src/project/Tenant.Portal/areas/Products/pages/ProductDetailPage/ProductDetailPage';

const Components = {
	HomePage,
	SignInPage,
	OrderPage,
	ClientPage,
	ClientDetailPage,
	ProductPage,
	ProductDetailPage
  };

export default (req, res, next) => {

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
}