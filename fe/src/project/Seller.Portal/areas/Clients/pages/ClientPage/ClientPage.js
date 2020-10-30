import React from "react";
import { Plus } from "react-feather";
import Header from "../../../../../../shared/components/Header/Header";
import Footer from "../../../../../../shared/components/Footer/Footer";
import MenuTiles from "../../../../../../shared/components/MenuTiles/MenuTiles";

function ClientPage(props) {
  return (
    <div>
      <Header {...props.header}></Header>
      <MenuTiles {...props.menuTiles} />
      <section className="section section-small-padding catalog">
        <h1 className="subtitle is-4">{props.title}</h1>
        <div>
            {props.newUrl &&
                <a href={props.newUrl} className="button is-primary">
                    <span className="icon">
                        <Plus />
                    </span>
                    <span>
                        {props.newText}
                    </span>
                </a>
            }
        </div>
      </section>
      <Footer {...props.footer}></Footer>
    </div>
  );
}

export default ClientPage;