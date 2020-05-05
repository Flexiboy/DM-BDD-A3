create database cooking;
use cooking;
set sql_safe_updates=0;

USE `cooking`;

CREATE TABLE `cooking`.`client` (
	`codeClient` VARCHAR(8) NOT NULL,
    `nomC` VARCHAR(20) NOT NULL,
    `prenomC` VARCHAR(20) NULL,
    `telephoneC` VARCHAR(10) NOT NULL,
    `usernameC` VARCHAR(20) NOT NULL,
    `mdpC` VARCHAR(20) NOT NULL,
	`createur` BOOLEAN NOT NULL DEFAULT False, -- Initialiser à faux
    `cook` FLOAT NOT NULL DEFAULT 0, -- Initialiser à 0
    `nombreCommandeCdR` INT NOT NULL DEFAULT 0,
    UNIQUE (`usernameC`), --  nom d'utilisateur unique pour pouvoir s'authentifier
	PRIMARY KEY (`codeClient`) );
    
CREATE TABLE `cooking`.`fournisseur` (
	`codeFournisseur` VARCHAR(8) NOT NULL,
    `nomF` VARCHAR(12) NOT NULL,
    `telephoneF` VARCHAR(10) NOT NULL,
    PRIMARY KEY (`codeFournisseur`) );
    
CREATE TABLE `cooking`.`produit` (
	`codeProduit` VARCHAR(8) NOT NULL,
    `nomP` VARCHAR(25) NOT NULL,
    `categorie` ENUM('boisson','fruit','légume','féculent','produit laitier','viande','poisson','oeuf','corps gras','sucre','épice','plante','autre'),
    `stock` INT NOT NULL,
    `stockMax` INT NOT NULL,
    `stockMin` INT NOT NULL,
    `unite` VARCHAR(8) NULL,
    `derniereUtilisation` DATE NOT NULL,
	PRIMARY KEY (`codeProduit`) );
    
CREATE TABLE `cooking`.`recette` (
	`codeRecette` VARCHAR(8) NOT NULL,
    `nomR` VARCHAR(50) NOT NULL,
    `type` ENUM('entrée', 'plat', 'dessert', 'fromage'),
    `descriptif` VARCHAR(256) NULL,
    `veg` BOOLEAN NOT NULL DEFAULT False,
    `prixR` FLOAT NOT NULL,
    `remuneration` FLOAT NULL DEFAULT 2,
    `codeClient` VARCHAR(8) NOT NULL DEFAULT 'C0000',
    `nombreCommandeSemaine` INT NOT NULL DEFAULT 0,
    `nombreCommande` INT NOT NULL DEFAULT 0,
	PRIMARY KEY (`codeRecette`), 
		INDEX `F_recette1_idx` (`codeClient` ASC),
		CONSTRAINT `codeClientRecette` FOREIGN KEY (`codeClient`)
			REFERENCES `cooking`.`client` (`codeClient`)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION );
    
CREATE TABLE `cooking`.`panier` (
	`codeCommande` VARCHAR(20) NOT NULL,
    `date` DATE NOT NULL,
    `prixP` FLOAT NOT NULL,
    `codeClient` VARCHAR(8) NOT NULL,
	PRIMARY KEY (`codeCommande`),
		INDEX `F_panier1_idx` (`codeClient` ASC),
		CONSTRAINT `codeClientPanier` FOREIGN KEY (`codeClient`)
			REFERENCES `cooking`.`client` (`codeClient`)
            ON DELETE CASCADE
            ON UPDATE NO ACTION );
            
CREATE TABLE `cooking`.`fournie` (
	`codeFournisseur` VARCHAR(8) NOT NULL,
    `codeProduit` VARCHAR(8) NOT NULL,
    PRIMARY KEY (`codeFournisseur`, `codeProduit`),
		INDEX `F_fournie1_idx` (`codeFournisseur` ASC),
		INDEX `F_fournie2_idx` (`codeProduit` ASC),
		CONSTRAINT `codeFournisseurFournie` FOREIGN KEY (`codeFournisseur`)
			REFERENCES `cooking`.`fournisseur` (`codeFournisseur`)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,
		CONSTRAINT `codeProduitFournie` FOREIGN KEY (`codeProduit`)
			REFERENCES `cooking`.`produit` (`codeProduit`)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION );
            
CREATE TABLE `cooking`.`constitutionRecette` (
	`codeRecette` VARCHAR(8) NOT NULL,
	`codeProduit` VARCHAR(8) NOT NULL,
	`quantiteProduit` FLOAT,
    PRIMARY KEY (`codeProduit`, `codeRecette`),
		INDEX `F_constitutionRecette1_idx` (`codeProduit` ASC),
        INDEX `F_constitutionRecette2_idx` (`codeRecette` ASC),
        CONSTRAINT `codeProduitConstitutionRecette` FOREIGN KEY (`codeProduit`)
			REFERENCES `cooking`.`produit` (`codeProduit`)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,
		CONSTRAINT `codeRecetteConstitutionRecette` FOREIGN KEY (`codeRecette`)
			REFERENCES `cooking`.`recette`(`codeRecette`)
            ON DELETE CASCADE
            ON UPDATE NO ACTION	);
            
CREATE TABLE `cooking`.`constitutionPanier` (
	`codeCommande` VARCHAR(20) NOT NULL,
	`codeRecette` VARCHAR(8) NOT NULL,
	`quantiteRecette` INT,
    PRIMARY KEY (`codeRecette`, `codeCommande`),
		INDEX `F_constitutionPanier1_idx` (`codeRecette` ASC),
        INDEX `F_constitutionPanier2_idx` (`codeCommande` ASC),
        CONSTRAINT `codeRecetteConstitutionPanier` FOREIGN KEY (`codeRecette`)
			REFERENCES `cooking`.`recette` (`codeRecette`)
            ON DELETE CASCADE
            ON UPDATE NO ACTION,
		CONSTRAINT `codeCommandeConstitutionPanier` FOREIGN KEY (`codeCommande`)
			REFERENCES `cooking`.`panier`(`codeCommande`)
            ON DELETE CASCADE
            ON UPDATE NO ACTION	);


-- PEUPLEMENT TABLE

-- (13) Insertion dans la table client
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0000','Cooking','Cooking','0000000000','Cooking','cooking', True, 0, 1);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0655','White','Walter','0685698521','IamTheDanger','BB_WW', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0656','Pinkman','Jesse','0645278962','bxxch','BB_PJ', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0657','Murdock','Matt','0732584115','Matt3S','mtmurdock', False, 0.4, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0658','Jones','Jessica','0647526364','JJ3S','jeyjey3', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0659','Son-Goku','Kakarot','0652489523','kameha','s_g_k', False, 12.6, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0660','Uzumaki','Naruto','0647523556','datebayo','raamen!', False, 6.0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0661','Lothbrok','Ragnar','0632869142','rgr_loth','peaceUpponKatt', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0662','Lothbrok','Lagertha','0683956782','TheShieldM','Lgtha', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0663','Grimes','Rick','0718252469','Rick','WeAreTheWD', False, 0, 0);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0664','Vinsmoke','Sanji','0684523476','kicker','naaami-chan', True, 76, 1);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0665','Ichigami','Senku','0638456237','Senku','keurScience', True, 32, 2);
INSERT INTO `cooking`.`client` (`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) VALUES ('C0666','Monkey D','Luffy','0677546729','pirateKing','8dz5e5f', False, 0, 0);


-- (4) Insertion dans la table fournisseur  
INSERT INTO `cooking`.`fournisseur` (`codeFournisseur`,`nomF`,`telephoneF`) VALUES ('F014','Johnson','0137869524');
INSERT INTO `cooking`.`fournisseur` (`codeFournisseur`,`nomF`,`telephoneF`) VALUES ('F015','Ichiraku','0148652391');
INSERT INTO `cooking`.`fournisseur` (`codeFournisseur`,`nomF`,`telephoneF`) VALUES ('F016','StarkFood','0117489635');
INSERT INTO `cooking`.`fournisseur` (`codeFournisseur`,`nomF`,`telephoneF`) VALUES ('F017','F&F','0136596842');


-- (29) Insertion dans la table produit  
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0014','Poulet','viande', 50000, 100000, 10000,'g', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0015','Oignon nouveau','légume', 403, 1000, 100,'pièce', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0016','Sachet Ramen','féculent', 200, 800, 50,'sachet', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0017','Gingembre','épice', 650, 1000, 100,'pièce', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0018','Coriandre','plante', 100, 150, 50,'bouquet', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0019','Soja','corps gras', 2500, 5000, 500,'cl', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0020','Saké','boisson', 20000, 500000, 10000,'cl', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0021','Sucre','sucre', 500000, 1000000, 250000,'g', '22-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0022','Champignon noir','légume', 7000, 15000, 2000,'g', '21-04-20'); -- Dans la catégorie légume car il se cuisinne comme tel
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0023','Oeuf','oeuf', 450, 800, 300,'pièce', '22-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0024','Bouillon cube de volaille','viande', 250, 500, 100,'sachet', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0025','Sachet légume soupe miso','légume', 600, 800, 300,'sachet', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0026',"Huile d'olive",'corps gras', 20000, 50000, 10000,'g', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0027','Spaghetti','féculent', 100000, 150000, 50000,'g', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0028','Oignon','légume', 500, 10000, 100,'pièce', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0029',"Gouse d'ail",'légume', 300, 7000, 100,'pièce', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0030','Carotte','légume', 300, 8000, 200,'pièce', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0031','Céleri','légume', 200, 1000, 50,'pièce', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0032','Tomate','légume', 400000, 500000, 100000,'g', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0033','Boeuf haché','viande', 300000, 500000, 100000,'g', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0034','Persil','plante', 130, 200, 50,'pièce', '01-01-01');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0035','Huile','corps gras', 30000, 60000, 15000,'g', '22-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0036','Fromage feta','produit laitier', 2500, 10000, 1000,'g', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0037','Concombre','fruit', 330, 500, 200,'pièce', '21-04-20'); -- botaniquement un fruit mais consommé comme un légume...
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0038','Poivron rouge','légume', 40, 100, 20,'pièce', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0039','Olive noir','fruit', 6000, 20000, 3000,'g', '21-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0040','Farine','féculent', 600000, 850000, 300000,'g', '22-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0041','Levure','autre', 9000, 30000, 5000,'g', '22-04-20');
INSERT INTO `cooking`.`produit` (`codeProduit`,`nomP`,`categorie`,`stock`,`stockMax`,`stockMin`,`unite`, `derniereUtilisation`) VALUES ('P0042','Lait','produit laitier', 40000, 100000, 20000,'ml', '22-04-20');


-- (4) Insertion dans la table recette  
INSERT INTO `cooking`.`recette` (`codeRecette`,`nomR`,`type`,`descriptif`,`veg`,`prixR`,`remuneration`,`codeClient`, `nombreCommandeSemaine`, `nombreCommande`) VALUES ('R0014','Ramen Soupe','plat','Ramen au poulet exaltante et sans lactose', False, 35, 2,'C0665', 2, 2); -- 5. La recette a été crée par Ichigami Senku et il sera rémunéré à hauteur de 40% de ce prix
INSERT INTO `cooking`.`recette` (`codeRecette`,`nomR`,`type`,`descriptif`,`veg`,`prixR`,`remuneration`,`codeClient`, `nombreCommandeSemaine`, `nombreCommande`) VALUES ('R0015','Spaghetti bolognaise','plat','Excellent spaghetti à la bolognaise', False, 27, 2, 'C0664', 0, 0);
INSERT INTO `cooking`.`recette` (`codeRecette`,`nomR`,`type`,`descriptif`,`veg`,`prixR`,`remuneration`,`codeClient`, `nombreCommandeSemaine`, `nombreCommande`) VALUES ('R0016','Salade feta','entrée','Salade feta très fraîche en été! - compatible avec les régimes végétariens', True, 20, 2, 'C0000', 1, 1);
INSERT INTO `cooking`.`recette` (`codeRecette`,`nomR`,`type`,`descriptif`,`veg`,`prixR`,`remuneration`,`codeClient`, `nombreCommandeSemaine`, `nombreCommande`) VALUES ('R0017','Pancake','dessert','Pancake simple et succulent made by Sanji', True, 12, 2, 'C0664', 1, 1); -- Recette crée par Sanji


-- (41) Insertion dans la table constitutionRecette 
	
    -- Ramen Soupe
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0014', 125);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0015', 0.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0016', 0.75);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0017', 0.75);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0018', 0.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0019', 2);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0020', 1.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0021', 20);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0022', 3.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0023', 1);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0024', 0.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0025', 0.75);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0014','P0026', 3);
	-- Fin "Ramen soupe"
    
	-- Spaghetti bolognaise
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0027', 125);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0028', 0.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0029', 0.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0030', 0.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0031', 0.25);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0032', 212.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0033', 125);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0034', 1);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0021', 1.3);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0015','P0035', 6);
	-- Fin spaghetti bolognaise
    
    -- Salade feta
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0036', 50);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0037', 0.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0032', 70);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0038', 0.125);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0039', 2.5);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0016','P0026', 9);
	-- Fin salade feta
    
    -- Pancake	
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0023', 1);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0021', 10);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0035', 6);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0040', 75);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0041', 6);
INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES ('R0017','P0042', 100);
	-- Fin pancake
    

-- (2) Insertion dans la table panier 
INSERT INTO `cooking`.`panier` (`codeCommande`,`date`,`prixP`,`codeClient`) VALUES ('CC0001','21-04-2020', 99,'C0660'); -- Naruto commande deux soupe au ramen ainsi qu'une salade feta / + cook pour le livreur ??
INSERT INTO `cooking`.`panier` (`codeCommande`,`date`,`prixP`,`codeClient`) VALUES ('CC0002','22-04-2020', 14,'C0656');


-- (3) Insertion dans la table constitutionPanier 
	
    -- Commande de Naruto
INSERT INTO `cooking`.`constitutionPanier` (`codeCommande`,`codeRecette`,`quantiteRecette`) VALUES ('CC0001','R0014', 2);
INSERT INTO `cooking`.`constitutionPanier` (`codeCommande`,`codeRecette`,`quantiteRecette`) VALUES ('CC0001','R0016', 1);
	-- Fin commande de Naruto
    
INSERT INTO `cooking`.`constitutionPanier` (`codeCommande`,`codeRecette`,`quantiteRecette`) VALUES ('CC0002','R0017', 1);


-- (29) Insertion dans la table fournie 
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0014');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0015');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F015','P0016');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F017','P0017');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0018');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F015','P0019');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F015','P0020');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0021');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0022');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F017','P0023');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0024');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F015','P0025');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0026');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0027');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0028');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0029');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0030');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F017','P0031');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0032');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0033');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0034');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0035');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0036');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0037');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0038');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F014','P0039');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F017','P0040');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F017','P0041');
INSERT INTO `cooking`.`fournie` (`codeFournisseur`,`codeProduit`) VALUES ('F016','P0042');


-- UTILISATEUR 

create user 'cookingmama'@'localhost' identified by 'coco' ;
grant all on cooking.* to 'cookingmama'@'localhost';
